﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;

namespace PowerToysTests
{
    [TestClass]
    public class PowerToysTrayTests : PowerToysSession
    {
        private bool isSettingsOpened;
        private bool isTrayOpened;

        [TestMethod]
        public void SettingsOpen()
        {
            OpenSettings();
            ShortWait();

            //check settings window opened
            WindowsElement settingsWindow = session.FindElementByName("PowerToys Settings");
            Assert.IsNotNull(settingsWindow);

            isSettingsOpened = true;
        }

        [TestMethod]
        public void SettingsOpenWithContextMenu()
        {
            //open tray
            trayButton.Click();
            isTrayOpened = true;

            //open PowerToys context menu
            WindowsElement pt = session.FindElementByName("PowerToys");
            Assert.IsNotNull(pt);

            new Actions(session).MoveToElement(pt).ContextClick().Perform();
            ShortWait();

            //open settings
            session.FindElementByXPath("//MenuItem[@Name=\"Settings\"]").Click();
            ShortWait();

            //check settings window opened
            WindowsElement settingsWindow = session.FindElementByName("PowerToys Settings");
            Assert.IsNotNull(settingsWindow);

            isSettingsOpened = true;
        }

        [TestMethod]
        public void PowerToysExit()
        {
            //open PowerToys context menu
            trayButton.Click();
            isTrayOpened = true;

            WindowsElement notificationOverflow = session.FindElementByName("Notification Overflow");
            AppiumWebElement overflowArea = notificationOverflow.FindElementByName("Overflow Notification Area");
            AppiumWebElement powerToys = overflowArea.FindElementByName("PowerToys");
            Assert.IsNotNull(powerToys);

            new Actions(session).MoveToElement(powerToys).ContextClick().Perform();
            ShortWait();

            //exit
            session.FindElementByXPath("//MenuItem[@Name=\"Exit\"]").Click();
            ShortWait();

            //check PowerToys exited
            powerToys = null;
            try
            {
                notificationOverflow = session.FindElementByName("Notification Overflow");
                overflowArea = notificationOverflow.FindElementByName("Overflow Notification Area");
                powerToys = overflowArea.FindElementByName("PowerToys");
            }
            catch (OpenQA.Selenium.WebDriverException)
            {
                //expected, PowerToys shouldn't be here
            }

            LaunchPowerToys();
            ShortWait();

            Assert.IsNull(powerToys);
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            isSettingsOpened = false;
            isTrayOpened = false;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (isSettingsOpened)
            {
                CloseSettings();
            }
            
            if (isTrayOpened)
            {
                trayButton.Click();
            }
        }
    }
}
