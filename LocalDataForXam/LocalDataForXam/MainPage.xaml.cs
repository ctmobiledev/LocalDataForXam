using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LocalDataForXam
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

        private async void btnRun_Clicked(object sender, EventArgs e)
        {

            // test whatever platform is active

            var localData = DependencyService.Get<ILocalDataForXam>();


            // add key-value pairs to localData

            await localData.SetData("Weather", "Fair");
            await localData.SetData("Temperature", "74");
            await localData.SetData("Winds", "SE-5");
            await localData.SetData("City", "Galveston");
            /**
            **/

            // remove key-value pairs from localData

            /**
            await localData.RemoveData("City");
            await localData.RemoveData("Winds");
            await localData.RemoveData("Nothing");
            **/


            // get values for each key and display

            String weather = await localData.GetData("Weather");
            String temperature = await localData.GetData("Temperature");
            String winds = await localData.GetData("Winds");
            String city = await localData.GetData("City");
            String dummy = await localData.GetData("Dummy");

            String result = weather + "\r" + temperature + "\r" + winds + "\r" + city;

            await DisplayAlert("Test Finished", result, "OK");
            await DisplayAlert("Value of 'dummy':", dummy.ToString(), "OK");

        }

    }

}
