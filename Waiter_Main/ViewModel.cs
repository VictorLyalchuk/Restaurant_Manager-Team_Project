using Data_Access_Entity.Entities;
using PropertyChanged;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Waiter_App.ViewModel_Models;

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
        private ObservableCollection<StringClass> _New;
        private ObservableCollection<StringClass> _Update;
        private ObservableCollection<IDClass> _Receipts;
        public ViewModel()
        {
            _Category = new ObservableCollection<Category>();
            _Orders = new ObservableCollection<Order>();
            _Product = new ObservableCollection<Product>();
            _ProductOrder = new ObservableCollection<ProductOrder>();
            _Table = new ObservableCollection<Table>();
            _Waiter = new ObservableCollection<Waiter>();
            _New = new ObservableCollection<StringClass>();
            _Update = new ObservableCollection<StringClass>();
            _Receipts = new ObservableCollection<IDClass>();
        }
        public IEnumerable<Category> Category => _Category;
        public IEnumerable<Order> Orders => _Orders;
        public IEnumerable<Product> Product => _Product;
        public IEnumerable<ProductOrder> ProductOrder => _ProductOrder;
        public IEnumerable<Table> Table => _Table;
        public IEnumerable<Waiter> Waiter => _Waiter;
        public IEnumerable<StringClass> New => _New;
        public IEnumerable<StringClass> Update => _Update;
        public IEnumerable<IDClass> Receipts => _Receipts;
        public Order SelectedOrder { get; set; }
        public IDClass SelectedRecepient { get; set; }
        public Table SelectedTable { get; set; }
        public void AddInCategory(Category category)
        {
            _Category.Add(category);
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
        public void AddInNew(StringClass _new)
        {
            _New.Add(_new);
        }
        public void RemoveInNew(StringClass _new)
        {
            _New.Remove(_new);
        }
        public void CleareInNew()
        {
            _New.Clear();
        }
        public void AddInUpdate(StringClass update)
        {
            _Update.Add(update);
        }
        public void RemoveInUpdate(StringClass update)
        {
            _Update.Remove(update);
        }
        public void CleareInUpdate()
        {
            _Update.Clear();
        }
        public void AddInReceipts(IDClass receipts)
        {
            _Receipts.Add(receipts);
        }
        public void RemoveInReceipts(IDClass receipts)
        {
            _Receipts.Remove(receipts);
        }
        public void CleareInReceipts()
        {
            _Receipts.Clear();
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
        public void RemoveCheck(IDClass iDClass)
        {
            _Receipts.Remove(iDClass);
        }
    }
}
