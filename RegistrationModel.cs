using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenTicket
{
    internal class RegistrationModel
    {
        //
        // atributos
        //
        private string _eventCode;
        private string _participantCpf;
        private DateTime _registrationDate;

        //
        // propriedades
        //
        public string EventCode
        {
            get { return _eventCode; }
            set { _eventCode = value; }
        }
        public string ParticipantCpf
        {
            get { return _participantCpf; }
            set { _participantCpf = value; }
        }
        public DateTime RegistrationDate
        {
            get { return _registrationDate; }
            set { _registrationDate = value; }
        }

        //
        // métodos
        //

        // construtor parametrizado
        public RegistrationModel(string eventCode, string participantCpf, DateTime registrationDate)
        {
            this._eventCode = eventCode;
            this._participantCpf = participantCpf;
            this._registrationDate = registrationDate;
        }

        // construtor padrão
        public RegistrationModel()
        {
            this._eventCode = "";
            this._participantCpf = "";
            this._registrationDate = DateTime.Today;
        }
    }
}
