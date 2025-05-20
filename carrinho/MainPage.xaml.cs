namespace carrinho
using System.Collections.ObjectModel;

public partial class MainPage : ContentPage
{
    ObservableCollection<Produto> Produtos = new();

    public MainPage()
    {
        InitializeComponent();
        listaProdutos.ItemsSource = Produtos;
    }

    private void OnAdicionarProdutoClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(lblDescricao.Text) ||
            string.IsNullOrWhiteSpace(entryValor.Text) ||
            !decimal.TryParse(entryValor.Text.Replace(",", "."), out decimal valor))
        {
            DisplayAlert("Erro", "Preencha os campos corretamente.", "OK");
            return;
        }

        Produtos.Add(new Produto
        {
            Descricao = lblDescricao.Text,
            Valor = valor
        });

        lblDescricao.Text = "";
        entryValor.Text = "";
        AtualizarNumeros();
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Produto produto)
        {
            string novaDesc = await DisplayPromptAsync("Editar", "Nova descrição:", initialValue: produto.Descricao);
            string novoVal = await DisplayPromptAsync("Editar", "Novo valor:", keyboard: Keyboard.Numeric, initialValue: produto.Valor.ToString());

            if (!string.IsNullOrWhiteSpace(novaDesc) && decimal.TryParse(novoVal.Replace(",", "."), out decimal novoValor))
            {
                produto.Descricao = novaDesc;
                produto.Valor = novoValor;

                listaProdutos.ItemsSource = null;
                listaProdutos.ItemsSource = Produtos;
                AtualizarNumeros();
            }
        }
    }

    private void OnExcluirClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.BindingContext is Produto produto)
        {
            Produtos.Remove(produto);
            AtualizarNumeros();
        }
    }

    private void AtualizarNumeros()
    {
        for (int i = 0; i < Produtos.Count; i++)
        {
            Produtos[i].Numero = (i + 1).ToString();
        }

        totalItensLabel.Text = $"Total: {Produtos.Count}";
    }
}

public class Produto
{
    public string Numero { get; set; }
    public string Descricao { get; set; }
    public decimal Valor { get; set; }

    public string ValorFormatado => Valor.ToString("C2");
}
}
