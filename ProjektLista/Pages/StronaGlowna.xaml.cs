using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;
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
            labelPoprzednieWyszukwania.IsVisible = true;
            ListaObecnychWyszukiwan.ItemsSource= Zapisane;
        }
        else
        {
            labelPoprzednieWyszukwania.IsVisible = false;
            List<Search> lista = Dane.Where(search => search.name.ToLower().StartsWith(Search.Text.ToLower())).ToList();
             if (CzyFiltrAktywny.IsChecked == true)
             {
                 var FiltrowanaLista = new List<Search>();
                if (FiltrKraj.Text != null)
                {
                    //dodac ;label sprawdzajacy
                    var listTymczasowa = new List<Search>();
                    foreach(var x in lista)
                    {
                        int k = 0;
                        bool czydodac = true;
                        var kraj = x.country.ToLower();
                        var filtrkrajowy = FiltrKraj.Text.ToLower();
                        while (k < kraj.Count() && k<filtrkrajowy.Count())
                        {
                            if (kraj[k] == filtrkrajowy[k])
                            {
                                k++;
                            }
                            else
                            {
                                czydodac=false;
                                break;
                            }
                        }
                        if (czydodac == true)
                        {
                            listTymczasowa.Add(x);
                        }
                    }
                    FiltrowanaLista = listTymczasowa;
                }

                if (FiltrCena.Text != null)
                {
                    int maksCena;
                    if (int.TryParse(FiltrCena.Text, out maksCena))
                    {
                        FiltrowanaLista = lista.Where(search => search.price <= maksCena).ToList();
                    }
                }

                if (FiltrOcena.Text !=null)
                {
                    int minOcena;
                    if(int.TryParse(FiltrOcena.Text, out minOcena)){

                        FiltrowanaLista = FiltrowanaLista.Where(search => search.review_scores_rating >= minOcena).ToList();
                    }
                }
                 ListaObecnychWyszukiwan.ItemsSource = FiltrowanaLista;
             }
             else
             {

            ListaObecnychWyszukiwan.ItemsSource = lista;
            }
        }
        
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
    }//zmiana filtor - wlacza i wylacza menu filtrow (filtr kraju, oceny, ceny)

    private void ZobaczSzczegoly(object sender, EventArgs e)
    {
        var z = sender as MenuItem;
        if (z != null && z.CommandParameter is Search item)
        {
            DisplayAlert($"{item.name}", $"Kraj: {item.country}\nLokalizacja: {item.smart_location}\nCena: {item.price}\nSrednia ocena: {item.review_scores_rating}\nIloœæ sypialni: {item.bedrooms}","OK");
            if (!Zapisane.Contains(item))
            {
                Zapisane.Add(item);
            }
        }
    }
    private void ZmienStan(object sender, EventArgs e)
    {
        var z = sender as MenuItem;
        if (z != null && z.CommandParameter is Search item)
        {
            if (ListaObecnychWyszukiwan.ItemsSource == Zapisane)
            {
                Zapisane.Remove(item);
                ListaObecnychWyszukiwan.ItemsSource = new List<Search>();
                ListaObecnychWyszukiwan.ItemsSource = Zapisane;
            }
        }
    }

    private void CheckboxKlikniety(object sender, CheckedChangedEventArgs e)
    {
        TextZostalZmieniony(new object(), new TextChangedEventArgs("",""));
    }
}