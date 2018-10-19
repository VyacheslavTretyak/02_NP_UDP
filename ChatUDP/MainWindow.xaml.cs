using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatUDP
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Color color = Color.FromRgb(255, 191, 128);
		ChatClientServer chat = new ChatClientServer();
		public MainWindow()
		{
			InitializeComponent();

				
			TextBoxLogin.GotFocus += TextBoxLogin_GotFocus;
			TextBoxLogin.LostFocus += TextBoxLogin_LostFocus;
			TextBoxLogin.TextChanged += TextBoxLogin_TextChanged;
			TextBoxMessage.GotFocus += TextBoxMessage_GotFocus;
			TextBoxMessage.LostFocus += TextBoxMessage_LostFocus;
			TextBoxMessage.TextChanged += TextBoxMessage_TextChanged;
			TextBoxLogin.Foreground = new SolidColorBrush(color);
			TextBoxLogin.Text = "Login";
			TextBoxMessage.Foreground = new SolidColorBrush(color);
			TextBoxMessage.Text = "Message";
			ButtonSend.Click += ButtonSend_Click;
			

			chat.showMessage = ShowMessage;
			chat.Start();
		}

		private void ButtonSend_Click(object sender, RoutedEventArgs e)
		{
			chat.Send($"{TextBoxLogin.Text} : {DateTime.Now.ToString()} \n {TextBoxMessage.Text}");
			TextBoxMessage.Text = "";
		}

		private void ShowMessage(string message)
		{
			Dispatcher.Invoke(() =>
			{
				ListBoxMessages.Items.Add(message);
				ListBoxMessages.ScrollIntoView(ListBoxMessages.Items.GetItemAt(ListBoxMessages.Items.Count - 1));
			});
			
		}

		private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
		{
			ButtonSend.IsEnabled = !(TextBoxLogin.Text == "Login" || TextBoxLogin.Text == "");
		}

		private void TextBoxMessage_TextChanged(object sender, TextChangedEventArgs e)
		{
			ButtonSend.IsEnabled = !(TextBoxMessage.Text == "Message" || TextBoxMessage.Text == "");
		}

		private void TextBoxMessage_LostFocus(object sender, RoutedEventArgs e)
		{
			RemoveFocus(sender, "Message");
		}
		private void TextBoxMessage_GotFocus(object sender, RoutedEventArgs e)
		{
			SetFocus(sender, "Message");
		}
		private void TextBoxLogin_LostFocus(object sender, RoutedEventArgs e)
		{
			RemoveFocus(sender, "Login");
		}
		private void TextBoxLogin_GotFocus(object sender, RoutedEventArgs e)
		{
			SetFocus(sender, "Login");
		}
		private void RemoveFocus(object sender, string text)
		{
			TextBox tb = sender as TextBox;
			if (tb.Text == "")
			{
				tb.Foreground = new SolidColorBrush(color);
				tb.Text = text;
			}
		}
		private void SetFocus(object sender, string text)
		{
			TextBox tb = sender as TextBox;
			if (tb.Text == text)
			{
				tb.Foreground = new SolidColorBrush(Colors.Black);
				tb.Text = "";
			}
		}
	}
}

