# LIA-Code-Review
A repository made to show off code for a potential internship.

Scripts: ZombieTypes & AIController:

Dessa scripts skrevs till spelet Signal Game som var till ett Game Jam i februari 2022. Jag gillar dessa scripts, då vi alla i gruppen var duktiga på att hålla vår kod kommenterad och tydlig med vad variabler och funktioner gör. Jag tycker att det är lätt att hitta i, och det är första gången jag fick använda NavMesh Agent i Unity samt första gången jag arbetade med utvecklare jag aldrig träffat förut (De var från skolor från Nederländerna och Finland). Under den här perioden började vi få lära oss om olika programming patterns, och jag och min grupp lyckades arbeta in ScriptableObjects och ObjectPools vilket kändes bra. Nu såhär efteråt tycker jag fortfarande att scriptsen håller god kvalitet, om någonting kanske man skulle kunnat arbeta lite mer modulärt i AIControllern.


Script : SaveManager:

En SaveManager jag skrev till ett projekt där uppgiften var att använda Firebase för att spara och ladda data. Jag valde att illustrera den sparade datan genom en highscore lista som presenterades i slutet av spelet. Eftersom det bara var jag själv som arbetade i projektet så har jag slarvat lite med att kommentera koden ordentligt, vilket gör att det är lite förvirrande för mig själv att läsa den nu bara några månader senare. Utöver att det är lite svårt att följa när man inte är helt insatt i projektet, så tycker jag ändå att koden är bra och effektiv. Den använder flera programming patterns och känns clean.


Scripts : BurgerOrders & OrderManager:

Ett nu pågående projekt i VR. BurgerOrders var det ursprungliga scriptet som skrevs, med målet att printa ut en order på en burgare med ingredienser på ett gameobject i spelet. Efter att ha arbetat med koden och lagt till möjligheten att se ikoner på varje ingrediens på gameobjectet, så blev det OrderManager. Ett snyggt och rent script som plockar random ingredienser och lägger dem på en order. Allt eftersom vi itererade idén och scriptet så valde vi att istället börja använda scriptableobjects vilket fungerat bra. 
Finns möjlighet för förbättring i foreach loopen som börjar på rad 20, där jag planerar att lägga ett till script på gameobjectet som listar TMPro samt Image som publika variabler så att man t.ex. istället för "orderListEntry.GetComponentInChildren<Image>().sprite = i.Icon;" kan korta ner det till "orderEntry.imageEntry = i.Icon;" vilket blir lite snyggare. Dåligt med kommentarer i det här scriptet, ett resultat av att vi omarbetat det och att jag och mina gruppkamrater tittat igenom det flera gånger och känner oss trygga i det.
