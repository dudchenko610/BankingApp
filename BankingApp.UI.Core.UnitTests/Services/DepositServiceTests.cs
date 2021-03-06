using BankingApp.UI.Core.Interfaces;
using BankingApp.ViewModels.Enums;
using Bunit;
using Moq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using BankingApp.UI.Core.Services;
using static BankingApp.Shared.Constants;
using System.Collections.Generic;
using BankingApp.Shared.Extensions;
using BankingApp.ViewModels.ViewModels.Deposit;
using BankingApp.ViewModels.ViewModels.Pagination;

namespace BankingApp.UI.Core.UnitTests.Services
{
    public class DepositServiceTests : TestContext
    {
        private const int ValidDepositId = 1;

        private DepositService _depositService;
        private Mock<IHttpService> _httpServiceMock;

        public DepositServiceTests()
        {
            _httpServiceMock = new Mock<IHttpService>();
            _depositService = new DepositService(_httpServiceMock.Object);
        }

        [Fact]
        public async Task Calculate_ValidCalculateView_PostAsyncInvoked()
        {
            var validCalculateView = GetValidCalculateView();
            CalculateDepositView depositViewPassedToHttpService = null;
            string passedUrl = null;

            _httpServiceMock.Setup(x => x.PostAsync<int, CalculateDepositView>(It.IsAny<string>(), It.IsAny<CalculateDepositView>(), It.IsAny<bool>()))
                .Callback((string url, CalculateDepositView val, bool authorized) => 
                    { 
                        passedUrl = url; 
                        depositViewPassedToHttpService =  val; 
                    })
                .ReturnsAsync(ValidDepositId);

            var depositId = await _depositService.CalculateAsync(validCalculateView);

            depositViewPassedToHttpService
                .Should().NotBeNull().And
                .BeOfType<CalculateDepositView>().And
                .BeEquivalentTo(validCalculateView);

            depositId.Should().Be(ValidDepositId);
            passedUrl.Should().Be($"{Routes.Deposit.Route}/{Routes.Deposit.Calculate}");
        }

        [Fact]
        public async Task GetAll_ValidPageDataView_GetAsyncInvoked()
        {
            const int ValidPageNumber = 1;
            const int ValidPageSize = 10;

            var getAsyncResponse = GetValidPageDataView();
            string passedUrl = null;

            _httpServiceMock.Setup(x => x.GetAsync<PagedDataView<DepositGetAllDepositViewItem>>(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback((string url, bool authorized) => { passedUrl = url; })
                .ReturnsAsync(getAsyncResponse);

            var pagedDataResponse = await _depositService.GetAllAsync(ValidPageNumber, ValidPageSize);

            pagedDataResponse.Should().Be(getAsyncResponse);
            passedUrl.Should().Be($"{Routes.Deposit.Route}/{Routes.Deposit.GetAll}?pageNumber={ValidPageNumber}&pageSize={ValidPageSize}");
        }

        [Fact]
        public async Task GetById_ValidGetByIdView_GetAsyncInvoked()
        {
            const int ValidDepositId = 1;

            var getByIdHttpServiceResponse = GetValidGetByIdView();
            string passedUrl = null;

            _httpServiceMock.Setup(x => x.GetAsync<GetByIdDepositView>(It.IsAny<string>(), It.IsAny<bool>()))
                .Callback((string url, bool authorized) => { passedUrl = url; })
                .ReturnsAsync(getByIdHttpServiceResponse);

            var getByIdDepositServiceResponse = await _depositService.GetByIdAsync(ValidDepositId);

            getByIdDepositServiceResponse.Should().Be(getByIdHttpServiceResponse);
            passedUrl.Should().Be($"{Routes.Deposit.Route}/{Routes.Deposit.GetById}?depositId={ValidDepositId}");
        }

        private CalculateDepositView GetValidCalculateView()
        {
            return new CalculateDepositView
            {
                DepositSum = 100.00m,
                Percents = 5,
                CalculationFormula = DepositCalculationFormulaEnumView.SimpleInterest,
                MonthsCount = 2
            };
        }

        private PagedDataView<DepositGetAllDepositViewItem> GetValidPageDataView()
        {
            return new PagedDataView<DepositGetAllDepositViewItem>
            {
                Items = new List<DepositGetAllDepositViewItem>
                {
                    new DepositGetAllDepositViewItem {
                        Id = 1,
                        DepositSum = 100,
                        CalculationFormula = DepositCalculationFormulaEnumView.SimpleInterest.GetDisplayValue(),
                        Percents = 2.45f
                    },
                    new DepositGetAllDepositViewItem {
                        Id = 2,
                        DepositSum = 200,
                        CalculationFormula = DepositCalculationFormulaEnumView.SimpleInterest.GetDisplayValue(),
                        Percents = 5
                    },
                    new DepositGetAllDepositViewItem {
                        Id = 3,
                        DepositSum = 300,
                        CalculationFormula = DepositCalculationFormulaEnumView.CompoundInterest.GetDisplayValue(),
                        Percents = 6
                    }
                },
                PageNumber = 1,
                PageSize = 10,
                TotalItems = 5
            };
        }

        private GetByIdDepositView GetValidGetByIdView()
        {
            return new GetByIdDepositView
            {
                Id = 1,
                CalculationFormula = DepositCalculationFormulaEnumView.CompoundInterest.GetDisplayValue(),
                CalсulationDateTime = System.DateTime.Now,
                DepositSum = 100.00m,
                MonthsCount = 4,
                Percents = 2.5f,
                MonthlyPaymentItems = new List<MonthlyPaymentGetByIdDepositViewItem>
                {
                    new MonthlyPaymentGetByIdDepositViewItem
                    {
                        MonthNumber = 1,
                        Percents = 0.5f,
                        TotalMonthSum = 30.00m
                    },
                    new MonthlyPaymentGetByIdDepositViewItem
                    {
                        MonthNumber = 2,
                        Percents = 1.5f,
                        TotalMonthSum = 70.00m
                    }
                }
            };
        }
    }
}
