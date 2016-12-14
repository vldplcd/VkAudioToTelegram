using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            botm = new BotManager();
            Dispatcher.Invoke(() => log_box.AppendText(""));
            botm.logevent += AppendLog;
            txb_messege.Text = "";
            Closing += botm.OnClosing;
        }

        private void AppendLog(string log)
        {
            
            Dispatcher.Invoke(() => log_box.AppendText(log));
        }

        private async void btn_start_bot_Click(object sender, RoutedEventArgs e)
        {
            await botm.StartBot();
            Dictionary<long, string> usernames = new Dictionary<long, string>();
            usernames = await botm.Usernames();

            Binding cmb_us_binding = new Binding();
            cmb_us_binding.Source = usernames;
            cmb_users.SetBinding(ItemsControl.ItemsSourceProperty, cmb_us_binding);            
        }

        private void btn_stop_bot_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
