using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenTicket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // instancia a classe Tela com as cores do sistema
            Tela minhaTela = new Tela(ConsoleColor.Black, ConsoleColor.Cyan);

            // instancia os controllers das entidades principais
            EventController eventController = new EventController(5, 4, minhaTela);
            ParticipantController participantController = new ParticipantController(5, 3, minhaTela);
            RegistrationController registrationController = new RegistrationController(5, 4, minhaTela,
                eventController, participantController);

            string opcao = "";
            List<string> opcoesMenu = new List<string>();
            opcoesMenu.Add("1 - Eventos                      ");
            opcoesMenu.Add("2 - Participantes                ");
            opcoesMenu.Add("3 - Inscrições                   ");
            opcoesMenu.Add("4 - Eventos com Vagas            ");
            opcoesMenu.Add("5 - Participantes por Evento     ");
            opcoesMenu.Add("0 - Sair                         ");

            while (true)
            {
                minhaTela.PrepararTela("EvenTicket — Controle de Eventos", 0, 0, 79, 24);
                opcao = minhaTela.MostrarMenu(2, 2, opcoesMenu);

                if (opcao == "0")
                {
                    // salva os dados em arquivo antes de encerrar (persistência)
                    eventController.SaveToFile();
                    participantController.SaveToFile();
                    registrationController.SaveToFile();

                    Console.Clear();
                    Console.WriteLine("EvenTicket encerrado. Dados salvos com sucesso.");
                    break;
                }

                if      (opcao == "1") eventController.CRUD();
                else if (opcao == "2") participantController.CRUD();
                else if (opcao == "3") registrationController.CRUD();
                else if (opcao == "4") registrationController.ReportAvailableEvents();
                else if (opcao == "5") registrationController.ReportParticipantsByEvent();
            }
        }
    }
}
