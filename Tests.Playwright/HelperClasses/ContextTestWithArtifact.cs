﻿using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Tests.Playwright.HelperClasses
{
    /* 
     * ContextTestWithArtifact is a base class for tests that require Playwright context and page.
     * It provides a setup and teardown methods for the test to run in a browser context.
     * It also provides a way to enable trace, screenshot, and video artifacts for the test.
     * Users need to inherit this class instead of ContextTest to write tests.
     */
    [TestFixture]
    public class ContextTestWithArtifact : ContextTest
    {

        // Declare Page
        public IPage Page { get; private set; } = null!;

        [SetUp]
        public async Task Setup()
        {
            TestContext.WriteLine("Browser: " + BrowserName);
            // Enable Trace
            await Context.Tracing.StartAsync(new()
            {
                Title = $"{TestContext.CurrentContext.Test.Name}",
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
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
            await Context.Tracing.StopAsync(new()
            {
                Path = tracePath
            });
            TestContext.AddTestAttachment(tracePath, description: "Trace");
            // Take a screenshot on error and add it as an attachment
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Error)
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

            // Enable video artifact and add it as an attachment, Context close is required to save the video
            await Context.CloseAsync();
            var videoPath = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "playwright-videos",
                $"{TestContext.CurrentContext.Test.Name}.{Guid.NewGuid()}.webm");
            if (Page.Video != null)
            {
                await Page.Video.SaveAsAsync(videoPath);
                TestContext.AddTestAttachment(videoPath, description: "Video");
            }
        }

        public override BrowserNewContextOptions ContextOptions()
        {
            // Video enable via context option overriding the default context option.
            var options = base.ContextOptions();
            if (options == null)
                options = new BrowserNewContextOptions();
            options.RecordVideoDir = ".playwright-videos"; // temp path to enable video recording
            return options;
        }
    }
}