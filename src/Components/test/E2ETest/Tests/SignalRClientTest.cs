// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using BasicTestApp;
using Microsoft.AspNetCore.Components.E2ETest.Infrastructure;
using Microsoft.AspNetCore.Components.E2ETest.Infrastructure.ServerFixtures;
using Microsoft.AspNetCore.E2ETesting;
using OpenQA.Selenium;
using TestServer;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Components.E2ETest.Tests
{
    public class SignalRClientTest : ServerTestBase<DevHostServerFixture<BasicTestApp.Program>>,
        IClassFixture<BasicTestAppServerSiteFixture<CorsStartup>>
    {
        private readonly ServerFixture _apiServerFixture;

        public SignalRClientTest(
            BrowserFixture browserFixture,
            DevHostServerFixture<BasicTestApp.Program> devHostServerFixture,
            BasicTestAppServerSiteFixture<CorsStartup> apiServerFixture,
            ITestOutputHelper output)
            : base(browserFixture, devHostServerFixture, output)
        {
            _serverFixture.PathBase = "/subdir";
            _apiServerFixture = apiServerFixture;
        }

        protected override void InitializeAsyncCore()
        {
            Navigate(ServerPathBase);
            Browser.MountTestComponent<SignalRClientComponent>();
            Browser.Exists(By.Id("signalr-client"));
        }

        [Fact]
        public void SignalRClientWorks()
        {
            Browser.FindElement(By.Id("hub-url")).SendKeys(
                new Uri(_apiServerFixture.RootUri, "/subdir/chathub").AbsoluteUri);
            Browser.FindElement(By.Id("hub-connect")).Click();

            Browser.Equal("SignalR Client: Echo",
                () => Browser.FindElements(By.CssSelector("li")).FirstOrDefault()?.Text);
        }
    }
}
