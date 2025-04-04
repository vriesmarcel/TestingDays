﻿using Azure.Developer.MicrosoftPlaywrightTesting.TestLogger;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.Playwright.TestAdapter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Tests.Playwright.HelperClasses
{
    /* 
     * PlaywrightTestWithArtifact is a base class for tests that require Playwright context and page.
     * It provides a setup and teardown methods for the test to run in a browser context.
     * It also provides a way to enable trace, screenshot, and video artifacts for the test.
     * Users need to inherit this class instead of PlaywrightTest to write tests.
     */
    [TestFixture]
    public class PlaywrightTestWithArtifact : PlaywrightTest
    {
        // Declare Browser, Context and Page
        public IPage Page { get; private set; } = null!;
        public IBrowserContext Context { get; private set; } = null!;
        public IBrowser Browser { get; private set; } = null!;

        public virtual BrowserNewContextOptions ContextOptions()
        {
            return new()
            {
                Locale = "en-US",
                ColorScheme = ColorScheme.Light,
                RecordVideoDir = ".videos"
            };
        }

        [SetUp]
        public async Task Setup()
        {

            var useCloudBrowsers = TestContext.Parameters.Get(RunSettingKey.UseCloudHostedBrowsers);

            if (useCloudBrowsers == "true")
            {
                /* Connect Remote Browser using BrowserType.ConnectAsync
                 * fetches service connect options like wsEndpoint and options
                 * add x-playwright-launch-options header to pass launch options likes channel, headless, etc.
                 */
                var playwrightService = new PlaywrightService();
                var connectOptions = await playwrightService.GetConnectOptionsAsync<BrowserTypeConnectOptions>();
                var launchOptionString = JsonSerializer.Serialize(PlaywrightSettingsProvider.LaunchOptions, new JsonSerializerOptions() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
                if (connectOptions.Options!.Headers != null)
                {
                    connectOptions.Options.Headers = connectOptions.Options.Headers.Concat(new Dictionary<string, string> { { "x-playwright-launch-options", launchOptionString } });
                }
                else
                {
                    connectOptions.Options.Headers = new Dictionary<string, string> { { "x-playwright-launch-options", launchOptionString } };
                }
                Browser = await BrowserType.ConnectAsync(connectOptions.WsEndpoint!, connectOptions.Options!);
            }
            else
            {
                Browser = await BrowserType.LaunchAsync(PlaywrightSettingsProvider.LaunchOptions);
            }

            // Create context and page
            Context = await Browser.NewContextAsync(ContextOptions());

            // Enable Trace
            await Context.Tracing.StartAsync(new()
            {
                Title = $"{TestContext.CurrentContext.Test.Name}",
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
            // Create a new page
            Page = await Context.NewPageAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            /* On Windows, Its possble that path exceed 255 characters.
             * if it does, feel free to change the path to a shorter one.
             * possibly removing test name.
             */
            // Stop trace and add it as an attachment
            var tracePath = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "playwright-traces",
                $"{TestContext.CurrentContext.Test.Name}.{Guid.NewGuid()}.zip"
            );
            if (Context != null)
            {
                await Context.Tracing.StopAsync(new()
                {
                    Path = tracePath
                });
            }
            if (File.Exists(tracePath))
            {
                TestContext.AddTestAttachment(tracePath, description: "Trace");
            }
            else
            {
                TestContext.WriteLine("Trace file not found, so not added to the logs");
            }
            // Take a screenshot on error and add it as an attachment
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Error && Page != null)
            {
                var screenshotPath = Path.Combine(
                    TestContext.CurrentContext.WorkDirectory,
                    "playwright-screenshot",
                    $"{TestContext.CurrentContext.Test.Name}.{Guid.NewGuid()}.png");
                await Page.ScreenshotAsync(new()
                {
                    Path = screenshotPath,
                });
                TestContext.AddTestAttachment(screenshotPath, description: "Screenshot");
            }
            else
            {
                TestContext.WriteLine("Not able to take a schreenshot as Page is null");
            }


            // Enable video artifact and add it as an attachment, Context close is required to save the video
            if (Context != null)
                await Context.CloseAsync();

            var videoPath = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "playwright-videos",
                $"{TestContext.CurrentContext.Test.Name}.{Guid.NewGuid()}.webm");
            if (Page?.Video != null)
            {
                await Page.Video.SaveAsAsync(videoPath);
                TestContext.AddTestAttachment(videoPath, description: "Video");
            }
            else
            {
                TestContext.WriteLine("Not able to save video as Page or Page.Video is null");
            }

            if (Browser != null)
                await Browser.CloseAsync();
        }
    }
}