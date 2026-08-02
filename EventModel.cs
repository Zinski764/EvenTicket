using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenTicket
{
    internal class EventModel
    {
        //
        // atributos
        //
        private string _code;
        private string _name;
        private string _location;
        private DateTime _date;
        private int _capacity;

        //
        // propriedades
        //
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }
        public int Capacity
        {
            get { return _capacity; }
            set { _capacity = value; }
        }

        //
        // métodos
        //

        // construtor parametrizado
        public EventModel(string code, string name, string location, DateTime date, int capacity)
        {
            this._code = code;
            this._name = name;
            this._location = location;
            this._date = date;
            this._capacity = capacity;
        }

        // construtor padrão
        public EventModel()
        {
            this._code = "";
            this._name = "";
            this._location = "";
            this._date = DateTime.Today;
            this._capacity = 0;
        }
    }
}
