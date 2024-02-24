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
    public class ProductSearchHandler : SearchHandler
    {
        //public static readonly BindableProperty Clients;
        public static readonly BindableProperty ProductsProperty
            = BindableProperty.Create("Products", typeof(ObservableCollection<Product>), typeof(ProductSearchHandler), null);
        public ObservableCollection<Product> Products { 
            get => (ObservableCollection<Product>)GetValue(ProductsProperty); 
            set => SetValue(ProductsProperty, value); 
        }
        //protected override void OnQueryChanged(string oldValue, string newValue)
        //{
        //    base.OnQueryChanged(oldValue, newValue);
        //    if (string.IsNullOrWhiteSpace(newValue))
        //    {
        //        ItemsSource = null;
        //    }
        //    else
        //    {
        //        ItemsSource = Products.Where(x => x.product_name.Contains(newValue)).ToList();
        //    }
        //}

        //protected override void OnItemSelected(object item)
        //{
        //    base.OnItemSelected(item);
        //}
    }
}
