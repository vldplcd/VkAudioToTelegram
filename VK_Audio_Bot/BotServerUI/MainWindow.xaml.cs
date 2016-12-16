using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VK_Audio_Bot.BotManager;

namespace BotServerUI
{
    /// <summary> 
    /// Interaction logic for MainWindow.xaml 
    /// </summary> 
    public partial class MainWindow : Window
    {
        BotManager botm;

        public MainWindow()
        {
            InitializeComponent();
            btn_start_bot.IsEnabled = true;
            btn_stop_bot.IsEnabled = false;
            botm = new BotManager();
            Dispatcher.Invoke(() => log_box.AppendText(""));
            botm.logevent += AppendLog;
            botm.SetGetter(true);
            txb_messege.Text = "";
            Closing += botm.OnClosing;
        }

        private void AppendLog(string log)
        {
            
            Dispatcher.Invoke(() => log_box.AppendText(log));
        }

        private async void btn_start_bot_Click(object sender, RoutedEventArgs e)
        {
            btn_stop_bot.IsEnabled = true;
            btn_start_bot.IsEnabled = false;
            await botm.StartBot();
            Dictionary<long, string> usernames = new Dictionary<long, string>();
            usernames = await botm.Usernames();
            Binding cmb_us_binding = new Binding();
            cmb_us_binding.Source = usernames;
            cmb_users.SetBinding(ItemsControl.ItemsSourceProperty, cmb_us_binding);                   
        }

        private void btn_stop_bot_Click(object sender, RoutedEventArgs e)
        {
            btn_stop_bot.IsEnabled = false;
            btn_start_bot.IsEnabled = true;
            botm.StopBot();

        }

        private async void btn_send_Click(object sender, RoutedEventArgs e)
        {
            switch ((long)cmb_users.SelectedValue)
            {
                case 0:
                    if (cmb_users.Items.Count != 0)
                    {
                        var chIDs = ((Dictionary<long, string>)cmb_users.ItemsSource);
                        foreach (var c in chIDs.Keys)
                        {
                            if (c > 0)
                                try
                                {
                                    await botm.SendMessage(c, txb_messege.Text);
                                }
                                catch (Exception ex)
                                {
                                    AppendLog($"\nFailed to send message to {c}. {ex.Message}");
                                }
                        }
                    }
                    break;

                default:
                    try
                    {
                        await botm.SendMessage((long)cmb_users.SelectedValue, txb_messege.Text);
                    }
                    catch (Exception ex)
                    {
                        AppendLog($"\nFailed to send message to {(long)cmb_users.SelectedValue}. {ex.Message}");
                    }
                    break;
            }
            txb_messege.Text = "";
        }

        private void cmb_users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void rbVK_Checked(object sender, RoutedEventArgs e)
        {
            botm.SetGetter(true);
        }

        private void rbMp3CC_Checked(object sender, RoutedEventArgs e)
        {
            botm.SetGetter(false);
        }
    }
}
