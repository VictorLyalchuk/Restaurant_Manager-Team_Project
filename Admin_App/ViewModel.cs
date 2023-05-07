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
        public Order SelectedOrder { get; set; }
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
        public void RemoveInOrders(Order order) 
        {  
            _Orders.Remove(order); 
        }
        public void ClearInOrders()
        {
            _Orders.Clear();
        }
        public void AddInProduct(Product product)
        {
            _Product.Add(product);
        }
        public void RemoveInProduct(Product product)
        {
            _Product.Remove(product);
        }
        public void AddInProductOrder(ProductOrder productorder)
        {
            _ProductOrder.Add(productorder);
        }
        public void RemoveInProductOrder(ProductOrder productorder)
        {
            _ProductOrder.Remove(productorder);
        }
        public void ClearInProductOrders()
        {
            _ProductOrder.Clear();
        }
        public void AddInTable(Table table)
        {
            _Table.Add(table);
        }
        public void AddInWaiter(Waiter waiter)
        {
            _Waiter.Add(waiter);
        }
        public void RemoveInWaiters(Waiter waiter)
        {
            _Waiter.Remove(waiter);
        }
        public List<int> GetProductId(int OrederId)
        {
            List<int> arr = new List<int>();
            foreach (var item in _ProductOrder)
            {
                if (item.OrderId == OrederId) arr.Add(item.ProductId);
            }
            return arr;
        }
        public void SetOrders(IEnumerable<Order> orders)
        {
            ClearInOrders();
            foreach (var item in orders)
            {
                AddInOrders(item);
            }
        }
        public void SetProductOrder(IEnumerable<ProductOrder> productorder)
        {
            ClearInProductOrders();
            foreach (var item in productorder)
            {
                AddInProductOrder(item);
            }
        }
        public void RemoveProductOrderToId(int orderId)
        {
            List<ProductOrder> deletelist = new List<ProductOrder>();
            foreach (var item in _ProductOrder)           
                if(item.OrderId == orderId)              
                    deletelist.Add(item);

            foreach (var item in deletelist)
                RemoveInProductOrder(item);

        }
    }
}
