using System;
using System.Collections.Generic;
using System.Linq;
using SistemaPedidos.Domain;
using SistemaPedidos.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace SistemaPedidos
{
    class Program
    {
        static void Main(string[] args)
        {
            using var db = new Data.ApplicationContext();

            //Faz a migração no db, não é indica em prod.
            db.Database.Migrate();
            
            //Aqui ele verifica se á migrações pendentes, retorna um inumerable
            var existe = db.Database.GetPendingMigrations().Any();
            if(existe)
            {
                // aplique uma regra de negocio aqui, tipo finalizar aplicação ou realizar algo
            }

            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarDados();
            //RemoverRegistro();
        }
        
        private static void RemoverRegistro()
        {
            using var db = new SistemaPedidos.Data.ApplicationContext();

            //var cliente = db.Clientes.Find(2);
            var cliente = new Cliente { Id = 3 };
            //db.Clientes.Remove(cliente);
            //db.Remove(cliente);
            db.Entry(cliente).State = EntityState.Deleted;

            db.SaveChanges();
        }

        private static void AtualizarDados()
        {
            //instânciando a base de dados:
            using var db = new Data.ApplicationContext();

            /*se fosse um registro especifico e soubesse o id poderia ser assim a chamada:
            var cliente = db.Clientes.Find(1);
            cliente.Nome = "Cliente alterado passo 1";*/

            var cliente = new Cliente
            {
                Id = 1
            };

            var clienteDesconectado = new
            {
                Nome = "Cliente Desconectado Passo 3",
                Telefone = "7966669999"
            };

            db.Attach(cliente);
            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);

            //db.Clientes.Update(cliente); //passo 1, método desnecessário.
            db.SaveChanges();
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            //está indo na base de dados e retornando todos os pedidos existentes na memória.
            var pedidos = db.Pedidos
                .Include(p => p.Itens) //aqui ele vai carregar tbm os itens do pedido
                .ThenInclude(p => p.Produto)
                .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                 {
                     new PedidoItem
                     {
                         ProdutoId = produto.Id,
                         Desconto = 0,
                         Quantidade = 1,
                         Valor = 10,
                     }
                 }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();

            //var consultaPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();

            //usando o AsNoTracking não será rastreado em memória, o EFcore já vai direto no db.
            var consultaPorMetodoLinq = db.Clientes.AsNoTracking()
                .Where(p => p.Id > 0)
                .OrderBy(p => p.Id)
                .ToList();

            foreach (var cliente in consultaPorMetodoLinq)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Id}");
                //executa a busca primeiro em o que está em memória utilizando o find, ela esta sendo trackeada
                //db.Clientes.Find(cliente.Id);
                //O metódo FirstOrDefault já vai direto no db
                db.Clientes.FirstOrDefault(p => p.Id == cliente.Id);
            }
        }

        private static void InserirDadosEmMassa()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Name = "Rafael Almeida",
                CEP = "99999000",
                Cidade = "Itabaiana",
                Estado = "SE",
                Telefone = "99000001111",
            };

            var listaClientes = new[]
            {
                new Cliente
                {
                    Name = "Teste 1",
                    CEP = "99999000",
                    Cidade = "Itabaiana",
                    Estado = "SE",
                    Telefone = "99000001115",
                },
                new Cliente
                {
                    Name = "Teste 2",
                    CEP = "99999000",
                    Cidade = "Itabaiana",
                    Estado = "SE",
                    Telefone = "99000001116",
                },
            };


            using var db = new Data.ApplicationContext();
            db.AddRange(produto, cliente);

            db.Set<Cliente>().AddRange(listaClientes);
            db.Clientes.AddRange(listaClientes);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            //aqui ele vai rastrear(mapear) os novos registros individualmente:
            using var db = new Data.ApplicationContext();
            /*Primeira forma de uso:
            db.Produtos.Add(produto);*/

            //segunda forma de uso, e a mais usada e indicada:
            db.Set<Produto>().Add(produto);
            
            /*Terceira forma, forçando o rastreamento da entidade:
            db.Entry(produto).State = EntityState.Added;

            //Quarta forma de uso:
            db.Add(produto);*/

            //Aqui ele salvará no db tudo que foi rastreado.
            var registros = db.SaveChanges();
            Console.WriteLine($"Total Registro(s): {registros}");
        }
    }
}