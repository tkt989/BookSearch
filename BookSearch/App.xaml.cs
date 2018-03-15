using Xamarin.Forms;
using Realms;

namespace BookSearch
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Application.Current != null)
            {
                var realm = Realm.GetInstance();
                Model.BookInfo.Realm = realm;
                Model.LibraryEntity.Init(realm);
            }
            MainPage = new NavigationPage(new Page.MainPage());
            //MainPage = new NavigationPage(new Page.Search.BookSearchPage("4822284611"));
        }

        public Realm GetRealm()
        {
            return (Realm)Properties["Realm"];
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
