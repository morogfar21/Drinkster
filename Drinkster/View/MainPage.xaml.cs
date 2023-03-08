using Drinkster.Model;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Drinkster;

public partial class MainPage : ContentPage
{
    ObservableCollection<Bars> bars = new ObservableCollection<Bars>();
    public ObservableCollection<Bars> Bars { get { return bars; } }

    public MainPage()
	{
		InitializeComponent();

        bars.Add(new Bars() { Name = "Old Irish", Address = "An apple is an edible fruit produced by an apple tree (Malus domestica)." });
        bars.Add(new Bars() { Name = "Hornslet", Address = "An apple is an edible fruit produced by an apple tree (Malus domestica)." });
        bars.Add(new Bars() { Name = "Bernhardts", Address = "An apple is an edible fruit produced by an apple tree (Malus domestica)." });
        bars.Add(new Bars() { Name = "Guldhornene", Address = "An apple is an edible fruit produced by an apple tree (Malus domestica)." });
        bars.Add(new Bars() { Name = "Guldkroen", Address = "An apple is an edible fruit produced by an apple tree (Malus domestica)." });
        bars.Add(new Bars() { Name = "Jægerkroen", Address = "An apple is an edible fruit produced by an apple tree (Malus domestica)." });
		
        barCollectionView.ItemsSource = bars;
    }
}


