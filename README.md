# DatabaseManageAplication
Projekt z Systemów Zarządzania Bazami Danych (PUT, Semestr 5).

Aplikacja zarządzająca dołączoną bazą danych wymyślonej firmy transportowej (w uproszczonej wersji). Logowanie odbywa się na dwóch poziomach dostępu: klienta i administratora.

Możliwości klienta:  
-zarejestrowanie się w systemie (jeżeli jeszcze tego nie zrobił),  
-złożenie zamówienia wybranych produktów firmy,  
-podejrzenie swoich dotychczasowych zamówień,  
-zmiana danych logowania oraz danych osobowych.  

Możliwości administratora:  
-dodawanie, modyfikowanie, oraz usuwanie wybranych elementów bazy danych (produktów, pojazdów, kierowców, klientów, etc.),  
-złożenie zamówienia w imieniu klienta,  
-anulowanie transportu (=automatyczne przesunięcie zamówień w czasie lub przypisanie ich do innego transportu - jeżeli zmierza w tym samym kierunku i jest miejsce w pojeździe).  

Uwaga - projekt zrealizowany na lokalnym serwerze w Microsoft SQL Server; aby móc prawidłowo korzystać z aplikacji, należy stworzyć lokalny serwer, załadować do niego dołączoną bazę danych, uruchomić projekt w Visual Studio, zmienić 'Data Source' w pliku Settings.settings na swoją nazwę serwera, a następnie zrekompilować projekt.

PS Projekt realizowany w dwuosobowym zespole, moim zadaniem było głównie przygotowanie bazy danych, tj. stworzenie schematu tabel oraz procedur składowanych.
