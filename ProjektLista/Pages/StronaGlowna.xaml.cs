using System.Linq;
using Newtonsoft.Json;
using ProjektLista.Classes;

namespace ProjektLista.Pages;

public partial class StronaGlowna : ContentPage
{
	List<Search> Dane = new List<Search>();
    List<Search> Zapisane= new List<Search>();
	public StronaGlowna()
	{
		InitializeComponent();
		PobierzJson();
	}

	public async void PobierzJson()
	{
        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("Jsons/AirbnbData.json");
            using var reader = new StreamReader(stream);
            string fileContent = await reader.ReadToEndAsync();
            Dane = JsonConvert.DeserializeObject<List<Search>>(fileContent);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        } 
	}


    private async void TextZostalZmieniony(object sender, TextChangedEventArgs e)
    {
        if (Search.Text == "")
        {
            ListaObecnychWyszukiwan.ItemsSource= Zapisane;
            return;
        }

        IEnumerable<Search> lista = Dane.Where(search => search.name.ToLower().StartsWith(Search.Text.ToLower()));
        ListaObecnychWyszukiwan.ItemsSource = lista;
    }

    public async void ZaznaczonoElement(object sender, SelectedItemChangedEventArgs e)
    {
        var z = e.SelectedItem as Search;
        if (Zapisane.Contains(z))
        {
            Zapisane.Remove(z);
            ListaObecnychWyszukiwan.ItemsSource=new List<Search>();
            ListaObecnychWyszukiwan.ItemsSource = Zapisane;
        }
        else
        {
            Zapisane.Add(z);
        }
        await DisplayAlert($"{z.name}",$"Lokalizacja: {z.country_code}, {z.street}\n\nOcena:{z.review_scores_rating}\n\nCena:{z.price}\n","Ok");
    }

    private void ZmianaFiltrow(object sender, EventArgs e)
    {
        if (MenuFiltrow.IsVisible == true)
        {
            MenuFiltrow.IsVisible = false;
        }
        else
        {
            MenuFiltrow.IsVisible = true;
        }
    }//gdy pokazuje te zapisane to jest filt a b c d, a jak wyszukuje to nie. zmiana filtor - wlacza i wylacza menu filtrow (filtr kraju, oceny, ceny)
}