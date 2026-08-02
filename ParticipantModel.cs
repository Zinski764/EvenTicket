using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenTicket
{
    internal class ParticipantModel
    {
        //
        // atributos
        //
        private string _cpf;
        private string _name;
        private string _email;

        //
        // propriedades
        //
        public string Cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        //
        // métodos
        //

        // construtor parametrizado
        public ParticipantModel(string cpf, string name, string email)
        {
            this._cpf = cpf;
            this._name = name;
            this._email = email;
        }

        // construtor padrão
        public ParticipantModel()
        {
            this._cpf = "";
            this._name = "";
            this._email = "";
        }
    }
}
