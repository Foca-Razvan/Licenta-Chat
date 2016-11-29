﻿using System;
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
using System.Windows.Shapes;

namespace Client.Windows
{
    /// <summary>
    /// Interaction logic for GroupConversationWindow.xaml
    /// </summary>
    public partial class GroupConversationWindow : Window
    {
        public List<string> Partners = new List<string>();
        public string GroupName { get; set;}

        public GroupConversationWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
            Height = 110;
            Width = 270;
            textBoxConversation.IsReadOnly = true;
        }

        public GroupConversationWindow(string groupName)
        {
            InitializeComponent();
            ResizeMode = ResizeMode.CanMinimize;
            Partners = ClientInformation.CommunicationService.GetGroupMembers(groupName);
            Partners.Remove(ClientInformation.Username);

            textBoxName.Visibility = Visibility.Hidden;
            buttonCreate.Visibility = Visibility.Hidden;
            textBoxConversation.IsReadOnly = true;
            GroupName = groupName;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AddFriendConversationWindow window = new AddFriendConversationWindow(Partners, GroupName);
            window.ShowDialog();
        }

        private void textBoxMessage_PressEnterDown(object sender,KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBoxConversation.Text += ClientInformation.Username + " " + DateTime.Now + ":" + textBoxMessage.Text + "\n";
                ClientInformation.CommunicationService.SendGroupMessage(ClientInformation.Username, GroupName, textBoxMessage.Text);
                textBoxMessage.Clear();
            }
        }

        private void buttonCall_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click(object sender,RoutedEventArgs e)
        {

        }

        public void UserJoined(string user)
        {
            textBoxConversation.Text += user + " has joined.\n";
            Partners.Add(user);
        }

        public void UserLeft(string user)
        {
            textBoxConversation.Text += user + " left.\n";
            Partners.Remove(user);
        }

        public void UserRefused(string user)
        {
            textBoxConversation.Text += user + " refused to join.\n";
        }

        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            GroupName = textBoxName.Text;

            if (ClientInformation.CommunicationService.CreateGroupConversation(ClientInformation.Username, GroupName))
            {
                Height = 374;
                Width = 562.67;                
                textBoxName.Visibility = Visibility.Hidden;
                buttonCreate.Visibility = Visibility.Hidden;

                ClientInformation.GroupConversationWindows.Add(GroupName, this);
            }        
        }

        public void ReceiveMessage(string sender,string message)
        {
            textBoxConversation.Text += sender + " " + DateTime.Now + ": " + message + "\n";
        }

        private void OnClosingEvent(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientInformation.CommunicationService.LeaveGroup(ClientInformation.Username, GroupName);
            ClientInformation.GroupConversationWindows.Remove(GroupName);
        }
    }
}