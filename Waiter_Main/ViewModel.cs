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
        private ObservableCollection<Order> _Orders;
        private ObservableCollection<StringClass> _New;
        private ObservableCollection<StringClass> _Update;
        private ObservableCollection<IDClass> _Receipts;
        public ViewModel()
        {
            _Orders = new ObservableCollection<Order>();
            _New = new ObservableCollection<StringClass>();
            _Update = new ObservableCollection<StringClass>();
            _Receipts = new ObservableCollection<IDClass>();
        }
        public IEnumerable<Order> Orders => _Orders;
        public IEnumerable<StringClass> New => _New;
        public IEnumerable<StringClass> Update => _Update;
        public IEnumerable<IDClass> Receipts => _Receipts;

        public Order SelectedOrder { get; set; }
        public IDClass SelectedRecepient { get; set; }
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
    }
}
