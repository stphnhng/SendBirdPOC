using System;
using SendBird;
using SendBird.SBJson;

using Xamarin.Forms;

namespace SBTest
{
    public class App : Application
    {
        Editor textBox;
        OpenChannel channel;
        public App()
        {
            var initSendbird = new Button
            {
                Text = "Initialize SendBird"
            };
            var connectSendbird = new Button
            {
                Text = "Connect to SendBird"
            };
            var testingButton = new Button
            {
                Text = "Press me to get a debug text"
            };
            var enterChannelButton = new Button
            {
                Text = "Enter Channel"
            };

            textBox = new Editor
            {
                Text = "Enter text here!"
            };

            testingButton.Clicked += (object sender, EventArgs e) =>
            {
                System.Diagnostics.Debug.WriteLine("Heres some text to prove that the button works");
            };

            initSendbird.Clicked += initButtonHandler;
            connectSendbird.Clicked += connectButtonHandler;
            enterChannelButton.Clicked += joinChannelHandler;

            ContentPage page = new ContentPage();
            StackLayout view = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center
            };
            page.Content = view;

            view.Children.Add(testingButton);
            view.Children.Add(initSendbird);
            view.Children.Add(connectSendbird);
            view.Children.Add(enterChannelButton);
            view.Children.Add(textBox);

            MainPage = page;
        }

        public void initButtonHandler(object sender, EventArgs e)
        {
            SendBird.SendBirdClient.Init("F438F6B9-4850-4452-80E8-8435C6BC35F6");
            System.Diagnostics.Debug.WriteLine("Success in initializing SendBird!");
        }

        public void connectButtonHandler(object sender, EventArgs e)
        {
            string userId = "admin";
            string accessToken = "92089f76d32b534685fc9dcf78bfc2bdc6e3b2b9";
            SendBird.SendBirdClient.Connect(userId, accessToken, (User user, SendBirdException sendException) =>
            {
                if (sendException != null)
                {
                    // An error has occurred while connecting.
                    System.Diagnostics.Debug.WriteLine("An error has occurred " +
                                                       "in connectButtonHandler while connecting");
                    System.Diagnostics.Debug.WriteLine("error: " + sendException.Code);
                }
                else
                {
                    // Success!
                    System.Diagnostics.Debug.WriteLine("Success in connecting to SendBird!");
                }

            });
        }

        public void joinChannelHandler(object sender, EventArgs e)
        {
            OpenChannel.GetChannel("testing", (channel, sendExcep) =>
            {
                if (sendExcep != null)
                {
                    // An error has occurred while connecting.
                    System.Diagnostics.Debug.WriteLine("An error has occurred " +
                                                       "in joinChannelHandler while joining");
                    System.Diagnostics.Debug.WriteLine("error: " + sendExcep.Code);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Success in joining a channel in SendBird!");
                }

                channel.Enter((SendBirdException enterExcep) =>
                {
                    if (enterExcep != null)
                    {
                        System.Diagnostics.Debug.WriteLine("An error has occurred " +
                                                           "in joinChannelHandler while entering");
                        System.Diagnostics.Debug.WriteLine("error: " + sendExcep.Code);
                    }
                });

                channel.SendUserMessage("Test", (message, sendUserTestE) =>
                {
                    if (sendUserTestE != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Error occurred when sending user message" +
                                                          " from inside joinChannelHandler");
                        System.Diagnostics.Debug.WriteLine("error: " + sendUserTestE.Code);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Successfully sent message");
                    }
                });
            });
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}