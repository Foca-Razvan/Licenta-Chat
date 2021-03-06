﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Client.Data
{
    public class FriendDataView
    {
        public ObservableCollection<FriendData> Items { get; set; }
        public List<FriendData> ModifiedItems { get; set; }

        public FriendDataView()
        {
            ModifiedItems = new List<FriendData>();

            Items = new ObservableCollection<FriendData>();
            Items.CollectionChanged += this.OnCollectionChanged;
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {            
            if (e.NewItems != null)
            {
                foreach (FriendData newItem in e.NewItems)
                {
                    ModifiedItems.Add(newItem);
                    newItem.PropertyChanged += this.OnItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (FriendData oldItem in e.OldItems)
                {
                    ModifiedItems.Add(oldItem);
                    oldItem.PropertyChanged -= this.OnItemPropertyChanged;
                }
            }
        }

        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {            
            FriendData item = sender as FriendData;
            if (item != null)
                ModifiedItems.Add(item);
        }

        public List<FriendData> GetOnlineFriends()
        {
            List<FriendData> list = new List<FriendData>();

            foreach (FriendData data in Items)
                if (data.Status)
                    list.Add(data);

            return list;
        }
    }
}
