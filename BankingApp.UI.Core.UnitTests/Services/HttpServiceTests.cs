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
using BankingApp.ViewModels.ViewModels.Authentication;
using BankingApp.UI.Core.UnitTests.TestModels;

namespace BankingApp.UI.Core.UnitTests.Services
{
    public class HttpServiceTests : TestContext
    {
        private const string TestUrl = "http://test.com/";
        private const string GetModel = "GetModel";
        private const string InternalServerErrorErrorMessage = "GetModel";

        private HttpService _httpService;
        private HttpClient _httpClient;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private Mock<INavigationWrapper> _navigationWrapperMock;
        private Mock<ILocalStorageService> _localStorageServiceMock;
        private Mock<IToastService> _toastServiceMock;

        public HttpServiceTests()
        {
            var validHttpResponse = GetValidHttpResponseMessage();
            var validTokensView = GetValidTokensView();

            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(validHttpResponse).Verifiable();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object) { BaseAddress = new Uri(TestUrl) };

            _navigationWrapperMock = new Mock<INavigationWrapper>();
            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()));

            _toastServiceMock = new Mock<IToastService>();
            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()));

            _localStorageServiceMock = new Mock<ILocalStorageService>();
            _localStorageServiceMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>())).ReturnsAsync(validTokensView);

            _httpService = new HttpService(_httpClient, _navigationWrapperMock.Object, _localStorageServiceMock.Object, _toastServiceMock.Object);
        }

        [Fact]
        public async Task Get_PassValidUrlWithUnauthorizedMode_ReturnsValidModelAndShowErrorShouldNotBeCalled()
        {
            bool unauthorizedMode = false;
            var validTestModel = GetValidResponseTestModel();
            string messageFromShowError = null;

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var fetchedTestModel = await _httpService.GetAsync<ResponseTestModel>(GetModel, unauthorizedMode);

            messageFromShowError.Should().BeNull();
            fetchedTestModel.Should().BeEquivalentTo(validTestModel);
        }

        [Fact]
        public async Task Get_PassValidUrlWithAuthorizedMode_CallsGetItemAsyncOfLocalStorageService()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidResponseTestModel();
            var validTokensView = GetValidTokensView();
            string accesTokenKey = null;

            _localStorageServiceMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .Callback((string key, CancellationToken? cancellationToken) => { accesTokenKey = key; }).ReturnsAsync(validTokensView);

            await _httpService.GetAsync<ResponseTestModel>(GetModel, authorizedMode);

            accesTokenKey.Should().Be(Constants.Constants.Authentication.TokensView);
        }

        [Fact]
        public async Task Get_PassValidUrlWithAuthorizedModeButServerRespondsWithStatusUnauthorized_CallsShowErrorAndNavigateToWithCorrespondingParameters()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidResponseTestModel();
            var unauthorizedHttpResponse = GetUnauthorizedHttpResponseMessage();

            string messageFromShowError = null;
            string logoutUri = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(unauthorizedHttpResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback((string uri, bool forceLoad) => { logoutUri = uri; });

            await _httpService.GetAsync<ResponseTestModel>(GetModel, authorizedMode);

            messageFromShowError.Should().Be(Constants.Constants.Notifications.Unauthorized);
            logoutUri.Should().Be(Constants.Constants.Routes.LogoutPage);
        }

        [Fact]
        public async Task Get_PassValidUrlModeButServerRespondsWithStatusInternalServerErrorAndErrorMessage_CallsShowErrorWithCorrespondingErrorMessage()
        {
            var validTestModel = GetValidResponseTestModel();
            var internalServerErrorResponse = GetInternalServerErrorHttpResponseMessageWithMessage();

            string messageFromShowError = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            await _httpService.GetAsync<ResponseTestModel>(GetModel, false);

            messageFromShowError.Should().Be(InternalServerErrorErrorMessage);
        }

        [Fact]
        public async Task Get_PassValidUrlModeButServerRespondsWithStatusInternalServerErrorWithoutErrorMessage_CallsShowErrorWithUnexpectedErrorMessage()
        {
            var validTestModel = GetValidResponseTestModel();
            var internalServerErrorResponse = GetInternalServerErrorHttpResponseMessageWithoutMessage();

            string messageFromShowError = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            await _httpService.GetAsync<ResponseTestModel>(GetModel, false);

            messageFromShowError.Should().Be(Constants.Constants.Notifications.UnexpectedError);
        }

        [Fact]
        public async Task Get_PassValidUrlModeButServerRespondsWithNotDeserializableBody_ReturnsEmptyModel()
        {
            var okResponse = GetOkResponseMessageWithNotDeserializableBody();

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(okResponse).Verifiable();

            var fetchedTestModel = await _httpService.GetAsync<ResponseTestModel>(GetModel, false);

            fetchedTestModel.Should().BeNull();
        }

        [Fact]
        public async Task Post_PassValidUrlWithUnauthorizedMode_ReturnsValidModelAndShowErrorShouldNotBeCalled()
        {
            bool unauthorizedMode = false;
            var validTestModel = GetValidResponseTestModel();
            string messageFromShowError = null;

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var validRequestTestModel = GetValidRequestTestModel();
            var fetchedTestModel = await _httpService.PostAsync<ResponseTestModel, RequestTestModel>(GetModel, validRequestTestModel, unauthorizedMode);

            messageFromShowError.Should().BeNull();
            fetchedTestModel.Should().BeEquivalentTo(validTestModel);
        }

        [Fact]
        public async Task Post_PassValidUrlWithAuthorizedMode_CallsGetItemAsyncOfLocalStorageService()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidResponseTestModel();
            var validTokensView = GetValidTokensView();
            string accesTokenKey = null;

            _localStorageServiceMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .Callback((string key, CancellationToken? cancellationToken) => { accesTokenKey = key; }).ReturnsAsync(validTokensView);

            var validRequestTestModel = GetValidRequestTestModel();
            await _httpService.PostAsync<ResponseTestModel, RequestTestModel>(GetModel, validRequestTestModel, authorizedMode);

            accesTokenKey.Should().Be(Constants.Constants.Authentication.TokensView);
        }

        [Fact]
        public async Task Post_PassValidUrlWithAuthorizedModeButServerRespondsWithStatusUnauthorized_CallsShowErrorAndNavigateToWithCorrespondingParameters()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidResponseTestModel();
            var unauthorizedHttpResponse = GetUnauthorizedHttpResponseMessage();

            string messageFromShowError = null;
            string logoutUri = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(unauthorizedHttpResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback((string uri, bool forceLoad) => { logoutUri = uri; });

            var validRequestTestModel = GetValidRequestTestModel();
            await _httpService.PostAsync<ResponseTestModel, RequestTestModel>(GetModel, validRequestTestModel, authorizedMode);

            messageFromShowError.Should().Be(Constants.Constants.Notifications.Unauthorized);
            logoutUri.Should().Be(Constants.Constants.Routes.LogoutPage);
        }

        [Fact]
        public async Task Post_PassValidUrlModeButServerRespondsWithStatusInternalServerErrorAndErrorMessage_CallsShowErrorWithCorrespondingErrorMessage()
        {
            var validTestModel = GetValidResponseTestModel();
            var internalServerErrorResponse = GetInternalServerErrorHttpResponseMessageWithMessage();

            string messageFromShowError = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var validRequestTestModel = GetValidRequestTestModel();
            await _httpService.PostAsync<ResponseTestModel, RequestTestModel>(GetModel, validRequestTestModel, false);

            messageFromShowError.Should().Be(InternalServerErrorErrorMessage);
        }

        [Fact]
        public async Task Post_PassValidUrlModeButServerRespondsWithStatusInternalServerErrorWithoutErrorMessage_CallsShowErrorWithUnexpectedErrorMessage()
        {
            var validTestModel = GetValidResponseTestModel();
            var internalServerErrorResponse = GetInternalServerErrorHttpResponseMessageWithoutMessage();

            string messageFromShowError = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var validRequestTestModel = GetValidRequestTestModel();
            await _httpService.PostAsync<ResponseTestModel, RequestTestModel>(GetModel, validRequestTestModel, false);

            messageFromShowError.Should().Be(Constants.Constants.Notifications.UnexpectedError);
        }

        [Fact]
        public async Task Post_PassValidUrlModeButServerRespondsWithNotDeserializableBody_ReturnsEmptyModel()
        {
            var okResponse = GetOkResponseMessageWithNotDeserializableBody();

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(okResponse).Verifiable();

            var validRequestTestModel = GetValidRequestTestModel();
            var fetchedTestModel = await _httpService.PostAsync<ResponseTestModel, RequestTestModel>(GetModel, validRequestTestModel, false);

            fetchedTestModel.Should().BeNull();
        }

        [Fact]
        public async Task PostEmptyResult_PassValidUrlWithUnauthorizedMode_ReturnsValidModelAndShowErrorShouldNotBeCalled()
        {
            bool unauthorizedMode = false;
            var validTestModel = GetValidResponseTestModel();
            string messageFromShowError = null;

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var validRequestTestModel = GetValidRequestTestModel();
            var responseResult = await _httpService.PostAsync<RequestTestModel>(GetModel, validRequestTestModel, unauthorizedMode);

            messageFromShowError.Should().BeNull();
            responseResult.Should().BeTrue();
        }

        [Fact]
        public async Task PostEmptyResult_PassValidUrlWithAuthorizedMode_CallsGetItemAsyncOfLocalStorageService()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidResponseTestModel();
            var validTokensView = GetValidTokensView();
            string accesTokenKey = null;

            _localStorageServiceMock.Setup(x => x.GetItemAsync<TokensView>(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .Callback((string key, CancellationToken? cancellationToken) => { accesTokenKey = key; }).ReturnsAsync(validTokensView);

            var validRequestTestModel = GetValidRequestTestModel();
            var responseResult = await _httpService.PostAsync<RequestTestModel>(GetModel, validRequestTestModel, authorizedMode);

            accesTokenKey.Should().Be(Constants.Constants.Authentication.TokensView);
            responseResult.Should().BeTrue();
        }

        [Fact]
        public async Task PostEmptyResult_PassValidUrlWithAuthorizedModeButServerRespondsWithStatusUnauthorized_CallsShowErrorAndNavigateToWithCorrespondingParameters()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidResponseTestModel();
            var unauthorizedHttpResponse = GetUnauthorizedHttpResponseMessage();

            string messageFromShowError = null;
            string logoutUri = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(unauthorizedHttpResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            _navigationWrapperMock.Setup(x => x.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback((string uri, bool forceLoad) => { logoutUri = uri; });

            var validRequestTestModel = GetValidRequestTestModel();
            var responseResult = await _httpService.PostAsync<RequestTestModel>(GetModel, validRequestTestModel, authorizedMode);

            messageFromShowError.Should().Be(Constants.Constants.Notifications.Unauthorized);
            logoutUri.Should().Be(Constants.Constants.Routes.LogoutPage);
            responseResult.Should().BeFalse();
        }

        [Fact]
        public async Task PostEmptyResult_PassValidUrlModeButServerRespondsWithStatusInternalServerErrorAndErrorMessage_CallsShowErrorWithCorrespondingErrorMessage()
        {
            var validTestModel = GetValidResponseTestModel();
            var internalServerErrorResponse = GetInternalServerErrorHttpResponseMessageWithMessage();

            string messageFromShowError = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var validRequestTestModel = GetValidRequestTestModel();
            var responseResult = await _httpService.PostAsync<RequestTestModel>(GetModel, validRequestTestModel, false);

            messageFromShowError.Should().Be(InternalServerErrorErrorMessage);
            responseResult.Should().BeFalse();
        }

        [Fact]
        public async Task PostEmptyResult_PassValidUrlModeButServerRespondsWithStatusInternalServerErrorWithoutErrorMessage_CallsShowErrorWithUnexpectedErrorMessage()
        {
            var validTestModel = GetValidResponseTestModel();
            var internalServerErrorResponse = GetInternalServerErrorHttpResponseMessageWithoutMessage();

            string messageFromShowError = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var validRequestTestModel = GetValidRequestTestModel();
            var responseResult = await _httpService.PostAsync<RequestTestModel>(GetModel, validRequestTestModel, false);

            messageFromShowError.Should().Be(Constants.Constants.Notifications.UnexpectedError);
            responseResult.Should().BeFalse();
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

        private ResponseTestModel GetValidResponseTestModel()
        {
            return new ResponseTestModel
            { 
                Id = 1,
                Value = "1"
            };
        }

        private TokensView GetValidTokensView()
        {
            return new TokensView
            {
                AccessToken = "access_token"
            };
        }

        private RequestTestModel GetValidRequestTestModel()
        {
            return new RequestTestModel
            { 
                Id = 1,
                Value = "value"
            };
        }
    }
}
