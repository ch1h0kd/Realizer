using CommunityToolkit.Maui.Views;
using Realizer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realizer.Resources.SearchHandlers
{
    public class ClientSearchHandler : SearchHandler
    {
        //public static readonly BindableProperty Clients;
        public static readonly BindableProperty ClientsProperty = 
            BindableProperty.Create("Clients", typeof(ObservableCollection<Client>), typeof(ClientSearchHandler), null);
        //public ObservableCollection<Client> Clients { get; set; }
        //protected  override void OnQueryChanged(string oldValue, string newValue)
        //{
        //    base.OnQueryChanged(oldValue, newValue);
        //    if(string.IsNullOrWhiteSpace(newValue))
        //    {
        //        ItemsSource = null;
        //    }
        //    else
        //    {
        //        ItemsSource = ClientsProperty.Where(x => x.Name.Contains(newValue)).ToList();
        //    }
        //}

        protected override void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
        }
    }
}
