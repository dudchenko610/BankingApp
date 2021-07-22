using BankingApp.UI.Core.Interfaces;
using BankingApp.UI.Core.Services;
using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Bunit;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using BankingApp.ViewModels.Banking.Authentication;

namespace BankingApp.UI.Core.UnitTests.Services
{
    public class HttpServiceTests : TestContext
    {
        private const string TestUrl = "http://test.com/";
        private const string GetModel = "GetModel";
        private const string InternalServerErrorErrorMessage = "GetModel";

        private HttpClient _httpClient;
        private INavigationWrapper _navigationWrapper;
        private ILocalStorageService _localStorageService;
        private IToastService _toastService;

        private class TestModel
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        public HttpServiceTests()
        {
            var validHttpResponse = GetValidHttpResponseMessage();
            var validTokensView = GetValidTokensView();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(validHttpResponse).Verifiable();
            _httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(TestUrl) };

            var navigationMamagerMock = new Mock<INavigationWrapper>();
            navigationMamagerMock.Setup(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()));
            _navigationWrapper = navigationMamagerMock.Object;

            var toastServiceMock = new Mock<IToastService>();
            toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()));
            _toastService = toastServiceMock.Object;

            var localStorageMock = new Mock<ILocalStorageService>();
            localStorageMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>())).ReturnsAsync(validTokensView);
            _localStorageService = localStorageMock.Object;
        }

        [Fact]
        public async Task Get_PassValidUrlWithUnauthorizedMode_ReturnsValidModelAndShowErrorShouldNotBeCalled()
        {
            bool unauthorizedMode = false;
            var validTestModel = GetValidTestModel();
            string messageFromShowError = null;

            var toastServiceMock = new Mock<IToastService>();
            toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var httpService = new HttpService(_httpClient, _navigationWrapper, _localStorageService, toastServiceMock.Object);
            var fetchedTestModel = await httpService.GetAsync<TestModel>(GetModel, unauthorizedMode);

            messageFromShowError.Should().BeNull();
            fetchedTestModel.Should().BeEquivalentTo(validTestModel);
        }

        [Fact]
        public async Task Get_PassValidUrlWithAuthorizedMode_CallsGetItemAsyncOfLocalStorageService()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidTestModel();
            var validTokensView = GetValidTokensView();
            string accesTokenKey = null;

            var localStorageMock = new Mock<ILocalStorageService>();
            localStorageMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .Callback((string key, CancellationToken? cancellationToken) => { accesTokenKey = key; }).ReturnsAsync(validTokensView);

            var httpService = new HttpService(_httpClient, _navigationWrapper, localStorageMock.Object, _toastService);
            await httpService.GetAsync<TestModel>(GetModel, authorizedMode);

            accesTokenKey.Should().Be(Constants.Constants.Authentication.TokensView);
        }

        [Fact]
        public async Task Get_PassValidUrlWithAuthorizedModeButServerRespondsWithStatusUnauthorized_CallsShowErrorAndNavigateToWithCorrespondingParameters()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidTestModel();
            var unauthorizedHttpResponse = GetUnauthorizedHttpResponseMessage();

            string messageFromShowError = null;
            string logoutUri = null;

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(unauthorizedHttpResponse).Verifiable();
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(TestUrl) };

            var toastServiceMock = new Mock<IToastService>();
            toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var navigationWrapperMock = new Mock<INavigationWrapper>();
            navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback((string uri, bool forceLoad) => { logoutUri = uri; });

            var httpService = new HttpService(httpClient, navigationWrapperMock.Object, _localStorageService, toastServiceMock.Object);
            await httpService.GetAsync<TestModel>(GetModel, authorizedMode);

            messageFromShowError.Should().Be(Constants.Constants.Notifications.Unauthorized);
            logoutUri.Should().Be(Constants.Constants.Routes.LogoutPage);
        }

        [Fact]
        public async Task Get_PassValidUrlModeButServerRespondsWithStatusInternalServerErrorAndErrorMessage_CallsShowErrorWithCorrespondingErrorMessage()
        {
            var validTestModel = GetValidTestModel();
            var internalServerErrorResponse = GetInternalServerErrorHttpResponseMessageWithMessage();

            string messageFromShowError = null;

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(TestUrl) };

            var toastServiceMock = new Mock<IToastService>();
            toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var httpService = new HttpService(httpClient, _navigationWrapper, _localStorageService, toastServiceMock.Object);
            await httpService.GetAsync<TestModel>(GetModel, false);

            messageFromShowError.Should().Be(InternalServerErrorErrorMessage);
        }

        [Fact]
        public async Task Get_PassValidUrlModeButServerRespondsWithStatusInternalServerErrorWithoutErrorMessage_CallsShowErrorWithUnexpectedErrorMessage()
        {
            var validTestModel = GetValidTestModel();
            var internalServerErrorResponse = GetInternalServerErrorHttpResponseMessageWithoutMessage();

            string messageFromShowError = null;

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(TestUrl) };

            var toastServiceMock = new Mock<IToastService>();
            toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var httpService = new HttpService(httpClient, _navigationWrapper, _localStorageService, toastServiceMock.Object);
            await httpService.GetAsync<TestModel>(GetModel, false);

            messageFromShowError.Should().Be(Constants.Constants.Notifications.UnexpectedError);
        }

        [Fact]
        public async Task Get_PassValidUrlModeButServerRespondsWithNotDeserializableBody_ReturnsEmptyModel()
        {
            var emptyTestModel = GetValidEmptyTestModel();
            var okResponse = GetOkResponseMessageWithNotDeserializableBody();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(okResponse).Verifiable();
            var httpClient = new HttpClient(handlerMock.Object) { BaseAddress = new Uri(TestUrl) };

            var httpService = new HttpService(httpClient, _navigationWrapper, _localStorageService, _toastService);
            var fetchedTestModel = await httpService.GetAsync<TestModel>(GetModel, false);

            fetchedTestModel.Should().BeEquivalentTo(emptyTestModel);
        }

        private HttpResponseMessage GetValidHttpResponseMessage()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"Id\":1,\"Value\":\"1\"}"),
            };
        }

        private HttpResponseMessage GetOkResponseMessageWithNotDeserializableBody()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{dfsfqq gd/.,}fd,"),
            };
        }

        private HttpResponseMessage GetUnauthorizedHttpResponseMessage()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("{}"),
            };
        }

        private HttpResponseMessage GetInternalServerErrorHttpResponseMessageWithMessage()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(InternalServerErrorErrorMessage),
            };
        }
        
        private HttpResponseMessage GetInternalServerErrorHttpResponseMessageWithoutMessage()
        {
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent(""),
            };
        }

        private TestModel GetValidTestModel()
        {
            return new TestModel
            { 
                Id = 1,
                Value = "1"
            };
        }

        private TestModel GetValidEmptyTestModel()
        {
            return new TestModel
            {
                Id = 0,
                Value = null
            };
        }

        private TokensView GetValidTokensView()
        {
            return new TokensView
            {
                AccessToken = "access_token"
            };
        }
    }
}
