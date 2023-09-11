using System;

namespace QrAmparoApi.Models
{
    public class Idoso
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Endereco { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Doenca { get; set; }
        public string Sexo { get; set; }
        public string TipoSanguineo { get; set; }
        public string NumeroSUS { get; set;}
        public ResponsavelQr Responsavel { get; set; }
        public int ResponsavelId { get; set; }
    }
}