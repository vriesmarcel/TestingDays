using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Tests.Playwright.HelperClasses;
namespace Tests.Playwright
{
    [TestFixture] 
    public class SimpleTests : PageTest
    {
        public string StartPage = "https://globoticket-frontend-dpfbe7hxa6d2bdab.westeurope-01.azurewebsites.net/";

        [Test] 
        public async Task BuyOneProduct()
        {
            //insert Generated code here
        }

        [Test]
        public async Task BuyTwoProducts()
        {
            await Page.GotoAsync("https://globoticket-frontend-dpfbe7hxa6d2bdab.westeurope-01.azurewebsites.net/");
            await Page.GetByRole(AriaRole.Row, new() { Name = "Artist pic 10/02/2025 John" }).GetByRole(AriaRole.Link).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "PLACE ORDER" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Back to event catalog" }).ClickAsync();
         
            await Page.GetByRole(AriaRole.Row, new() { Name = "Artist pic 01/02/2026 The" }).GetByRole(AriaRole.Link).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "PLACE ORDER" }).ClickAsync();

            await Page.GetByRole(AriaRole.Link, new() { Name = "CHECKOUT" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name" }).FillAsync("Marcel");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("marce.devries@xebia.com");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Address" }).FillAsync("Laapersveld 27");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Address" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Town" }).FillAsync("Hilversum");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Town" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Postal Code" }).FillAsync("1213VB");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Postal Code" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Credit Card" }).FillAsync("1111222233334444");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Credit Card" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Expiry Date" }).FillAsync("12/26");
            await Page.GetByRole(AriaRole.Button, new() { Name = "SUBMIT ORDER" }).ClickAsync();
            await Expect(Page.GetByRole(AriaRole.Heading)).ToContainTextAsync("Thank you for your order!");
        }

        [Test]
        public async Task SelectTreeTicketsAndRemoveOne()
        {
            //insert Generated code here
            await Page.GotoAsync("https://globoticket-frontend-dpfbe7hxa6d2bdab.westeurope-01.azurewebsites.net/");
            await Page.GetByRole(AriaRole.Row, new() { Name = "Artist pic 10/02/2025 John" }).GetByRole(AriaRole.Link).ClickAsync();
            await Page.GetByRole(AriaRole.Button, new() { Name = "PLACE ORDER" }).ClickAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Back to event catalog" }).ClickAsync();

            await Page.GetByRole(AriaRole.Row, new() { Name = "Artist pic 01/02/2026 The" }).GetByRole(AriaRole.Link).ClickAsync();
            await Page.GetByRole(AriaRole.Combobox).SelectOptionAsync(new[] { "3" });
            await Page.GetByRole(AriaRole.Button, new() { Name = "PLACE ORDER" }).ClickAsync();

            await Page.GetByRole(AriaRole.Link, new() { Name = "CHECKOUT" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name" }).ClickAsync();
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name" }).FillAsync("Marcel");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Name" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("marce.devries@xebia.com");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Address" }).FillAsync("Laapersveld 27");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Address" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Town" }).FillAsync("Hilversum");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Town" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Postal Code" }).FillAsync("1213VB");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Postal Code" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Credit Card" }).FillAsync("1111222233334444");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Credit Card" }).PressAsync("Tab");
            await Page.GetByRole(AriaRole.Textbox, new() { Name = "Expiry Date" }).FillAsync("12/26");
            await Page.GetByRole(AriaRole.Button, new() { Name = "SUBMIT ORDER" }).ClickAsync();
            await Expect(Page.GetByRole(AriaRole.Heading)).ToContainTextAsync("Thank you for your order!");

        }
    }
}