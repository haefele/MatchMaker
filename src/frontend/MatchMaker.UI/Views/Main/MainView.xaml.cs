﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.UI.Services.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatchMaker.UI.Views.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainView : ContentPage
	{
        public MainViewModel ViewModel => this.BindingContext as MainViewModel;

		public MainView ()
		{
			this.InitializeComponent();

            this.BindingContext = new MainViewModel(DependencyService.Get<INavigationService>());
		}
	}
}