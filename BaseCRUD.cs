using System;
using System.Collections.Generic;

namespace EvenTicket
{
    internal abstract class BaseCRUD<TModel>
    {

        // posição inicial da interface na tela e dimensões da interface da tela
        protected int _column, _row, _width, _heigth, _position;
        // rótulos dos campos apresentados na tela
        protected List<string> _fields;
        protected List<TModel> _items;
        // referência para a tela principal
        protected Tela _tela;

        public List<TModel> Items
        {
            get { return this._items; }
        }

        // Inicializa os atributos comuns utilizados pelos Controllers.
        public BaseCRUD(int col, int row, Tela tela)
        {
            this._column = col;
            this._row = row;
            this._tela = tela;
            this._fields = new List<string>();
            this._items = new List<TModel>();
        }

        public int Count()
        {
            return this._items.Count;
        }

        protected void AddItem(TModel item)
        {
            this._items.Add(item);
        }

        protected void RemoveItem(int position)
        {
            this._items.RemoveAt(position);
        }

        // Método abstrato responsável por executar o fluxo completo
        // de cadastro, consulta, alteração e exclusão.
        public abstract void CRUD();

        protected abstract void ShowForm();

        protected abstract void EnterData(string which);

        protected abstract void ShowData();
    }
}
