using Data_Access_Entity.Entities;
using PropertyChanged;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Waiter_App
{
    [AddINotifyPropertyChangedInterface]
    class ViewModel
    {
        private ObservableCollection<Category> _Category;
        private ObservableCollection<Order> _Orders;
        private ObservableCollection<Product> _Product;
        private ObservableCollection<ProductOrder> _ProductOrder;
        private ObservableCollection<Table> _Table;
        private ObservableCollection<Waiter> _Waiter;
        public ViewModel()
        {
            _Category = new ObservableCollection<Category>();
            _Orders = new ObservableCollection<Order>();
            _Product = new ObservableCollection<Product>();
            _ProductOrder = new ObservableCollection<ProductOrder>();
            _Table = new ObservableCollection<Table>();
            _Waiter = new ObservableCollection<Waiter>();
        }
        public IEnumerable<Category> Category => _Category;
        public IEnumerable<Order> Orders => _Orders;
        public IEnumerable<Product> Product => _Product;
        public IEnumerable<ProductOrder> ProductOrder => _ProductOrder;
        public IEnumerable<Table> Table => _Table;
        public IEnumerable<Waiter> Waiter => _Waiter;
        public void AddInCategory(Category category)
        {
            _Category.Add(category);
        }
        public void RemoveInCategory(Category category)
        {
            _Category.Remove(category);
        }
        public void AddInOrders(Order order)
        {
            _Orders.Add(order);
        }
        public void AddInProduct(Product product)
        {
            _Product.Add(product);
        }
        public void RemoveInProduct(Product product)
        {
            _Product.Remove(product);
        }
        public void AddInWaiter(Waiter waiter)
        {
            _Waiter.Add(waiter);
        }
        public void RemoveInWaiters(Waiter waiter)
        {
            _Waiter.Remove(waiter);
        }

    }
}
