using Data_Access_Entity.Entities;
using PropertyChanged;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Waiter_App
{
    [AddINotifyPropertyChangedInterface]
    class ViewModel
    {
        private ObservableCollection<Order> _Orders;
        private ObservableCollection<string> _New;
        private ObservableCollection<string> _Update;
        private ObservableCollection<int> _Receipts;
        public ViewModel()
        {
            _Orders = new ObservableCollection<Order>();
            _New = new ObservableCollection<string>();
            _Update = new ObservableCollection<string>();
            _Receipts = new ObservableCollection<int>();
        }
        public IEnumerable<Order> Orders => _Orders;
        public IEnumerable<string> New => _New;
        public IEnumerable<string> Update => _Update;
        public IEnumerable<int> Receipts => _Receipts;
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
        public void AddInNew(string _new)
        {
            _New.Add(_new);
        }
        public void RemoveInNew(string _new)
        {
            _New.Remove(_new);
        }
        public void CleareInNew()
        {
            _New.Clear();
        }
        public void AddInUpdate(string update)
        {
            _Update.Add(update);
        }
        public void RemoveInUpdate(string update)
        {
            _Update.Remove(update);
        }
        public void CleareInUpdate()
        {
            _Update.Clear();
        }
        public void AddInReceipts(int receipts)
        {
            _Receipts.Add(receipts);
        }
        public void RemoveInReceipts(int receipts)
        {
            _Receipts.Remove(receipts);
        }
        public void CleareInReceipts()
        {
            _Receipts.Clear();
        }
    } 

}
