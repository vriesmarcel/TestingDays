using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Tests.Playwright.HelperClasses;
using Tests.Playwright.Solid.PageObjects;
namespace Tests.Playwright
{
    [TestFixture]
    public class SimpleTests : PageTest
    {
        public string StartPage = "https://globoticket-frontend-dpfbe7hxa6d2bdab.westeurope-01.azurewebsites.net/";


        [Test]
        public async Task BuyOneProduct()
        {
            var homePage = new HomePage(Page);
            var productPage = new ProductPage(Page);

            await homePage.GotoHomepage(StartPage);
            await homePage.SelectProduct("John Egbert Live");

            await productPage.GotoCheckout();

            var checkoutPage = new CheckoutPage(Page);
            await checkoutPage.Checkout();
        }

        [Test]
        public async Task BuyTwoProducts()
        {
            //insert Generated code here
            var homePage = new HomePage(Page);
            var productPage = new ProductPage(Page);
            await homePage.GotoHomepage(StartPage);

            await homePage.SelectProduct("John Egbert Live");
            await productPage.BacktoCatalog();

            await homePage.SelectProduct("The State of Affairs");
            await productPage.GotoCheckout();

            var checkoutPage = new CheckoutPage(Page);
            await checkoutPage.Checkout();

        }

        [Test]
        public async Task SelectTreeTicketsAndRemoveOne()
        {
            //insert Generated code here
            var homePage = new HomePage(Page);
            var productPage = new ProductPage(Page);

            await homePage.GotoHomepage(StartPage);
            await homePage.SelectProduct("John Egbert Live");

            await productPage.BacktoCatalog();

            await homePage.SelectProduct("The State of Affairs", 3);
            await productPage.GotoCheckout();

            var checkoutPage = new CheckoutPage(Page);
            await checkoutPage.Checkout();
        }


    }
}