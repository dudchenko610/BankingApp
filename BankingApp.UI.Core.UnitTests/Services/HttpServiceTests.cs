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
            var validHttpResponse = GetHttpResponseMessage(HttpStatusCode.OK, new StringContent("{\"Id\":1,\"Value\":\"1\"}"));
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
        public async Task Get_ValidResponseTestModel_ExpectedResults()
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
        public async Task Get_AuthorizedMode_GetItemAsyncInvoked()
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
        public async Task Get_UnauthorizedStatusCode_ExpectedResults()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidResponseTestModel();
            var unauthorizedHttpResponse = GetHttpResponseMessage(HttpStatusCode.Unauthorized, new StringContent("{}"));

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

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError, InternalServerErrorErrorMessage, InternalServerErrorErrorMessage)] // NotSucceededResultWithErrorMessage
        [InlineData(HttpStatusCode.InternalServerError, "", Constants.Constants.Notifications.UnexpectedError)]  // NotSucceededResultWithoutErrorMessage
        public async Task Get_InternalServerErrorReturned_ShowErrorInvoked(HttpStatusCode httpStatusCode, string stringContent, string expectedError)
        {
            var validTestModel = GetValidResponseTestModel();
            var internalServerErrorResponse = GetHttpResponseMessage(httpStatusCode, new StringContent(stringContent));

            string messageFromShowError = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            await _httpService.GetAsync<ResponseTestModel>(GetModel, false);

            messageFromShowError.Should().Be(expectedError);
        }

        [Fact]
        public async Task Get_ResponseBodyIsNotDeserializable_ExpectedResults()
        {
            var okResponse = GetHttpResponseMessage(HttpStatusCode.OK, new StringContent("{dfsfqq gd/.,}fd,"));

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(okResponse).Verifiable();

            var fetchedTestModel = await _httpService.GetAsync<ResponseTestModel>(GetModel, false);

            fetchedTestModel.Should().BeNull();
        }

        [Fact]
        public async Task Post_UnauthorizedMode_ExpectedResults()
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
        public async Task Post_AuthorizedMode_GetItemAsyncInvoked()
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
        public async Task Post_UnauthorizedStatusCode_ExpectedResults()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidResponseTestModel();
            var unauthorizedHttpResponse = GetHttpResponseMessage(HttpStatusCode.Unauthorized, new StringContent("{}"));

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

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError, InternalServerErrorErrorMessage, InternalServerErrorErrorMessage)] // NotSucceededResultWithErrorMessage
        [InlineData(HttpStatusCode.InternalServerError, "", Constants.Constants.Notifications.UnexpectedError)]  // NotSucceededResultWithoutErrorMessage
        public async Task Post_InternalServerErrorReturned_ShowErrorInvoked(HttpStatusCode httpStatusCode, string stringContent, string expectedError)
        {
            var validTestModel = GetValidResponseTestModel();
            var internalServerErrorResponse = GetHttpResponseMessage(httpStatusCode, new StringContent(stringContent));

            string messageFromShowError = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var validRequestTestModel = GetValidRequestTestModel();
            await _httpService.PostAsync<ResponseTestModel, RequestTestModel>(GetModel, validRequestTestModel, false);

            messageFromShowError.Should().Be(expectedError);
        }

        [Fact]
        public async Task Post_ResponseBodyIsNotDeserializable_ExpectedResults()
        {
            var okResponse = GetHttpResponseMessage(HttpStatusCode.OK, new StringContent("{dfsfqq gd/.,}fd,"));

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(okResponse).Verifiable();

            var validRequestTestModel = GetValidRequestTestModel();
            var fetchedTestModel = await _httpService.PostAsync<ResponseTestModel, RequestTestModel>(GetModel, validRequestTestModel, false);

            fetchedTestModel.Should().BeNull();
        }

        [Fact]
        public async Task PostEmptyResult_UnauthorizedMode_ExpectedResults()
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
        public async Task PostEmptyResult_AuthorizedMode_GetItemAsyncInvoked()
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
        public async Task PostEmptyResult_UnauthorizedStatusCode_ExpectedResults()
        {
            bool authorizedMode = true;
            var validTestModel = GetValidResponseTestModel();
            var unauthorizedHttpResponse = GetHttpResponseMessage(HttpStatusCode.Unauthorized, new StringContent("{}"));

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

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError, InternalServerErrorErrorMessage, InternalServerErrorErrorMessage)] // NotSucceededResultWithErrorMessage
        [InlineData(HttpStatusCode.InternalServerError, "", Constants.Constants.Notifications.UnexpectedError)]  // NotSucceededResultWithoutErrorMessage
        public async Task PostEmptyResult_InternalServerErrorReturned_ShowErrorInvoked(HttpStatusCode httpStatusCode, string stringContent, string expectedError)
        {
            var validTestModel = GetValidResponseTestModel();
            var internalServerErrorResponse = GetHttpResponseMessage(httpStatusCode, new StringContent(stringContent));

            string messageFromShowError = null;

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(internalServerErrorResponse).Verifiable();

            _toastServiceMock.Setup(x => x.ShowError(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string message, string heading) => { messageFromShowError = message; });

            var validRequestTestModel = GetValidRequestTestModel();
            var responseResult = await _httpService.PostAsync<RequestTestModel>(GetModel, validRequestTestModel, false);

            messageFromShowError.Should().Be(expectedError);
            responseResult.Should().BeFalse();
        }

        private HttpResponseMessage GetHttpResponseMessage(HttpStatusCode statusCode, HttpContent httpContent)
        {
            return new HttpResponseMessage()
            {
                StatusCode = statusCode,
                Content = httpContent,
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
