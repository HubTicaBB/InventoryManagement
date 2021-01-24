# InventoryManagement

by **Tijana Ilic**

## Designmönster: Repository Pattern
Jag har valt att implemmentera Repository Pattern designmönster som är ett vanligt mönster för att hantera data access i sin applikation. Repository Pattern enkapsulerar logiken som kommunicerar med datalagringslager så att användaren (här: controller) inte bryr sig om hur kommunikationen med databasen sker (t ex genom Entity Framework, en annan ORM, filsystem osv). Utan en sådan mellanlager är controller tätt kopplad med datalagringslager.

En av fördelarna med det här designmönstret är att den gör applikationen mer skalbar. Det blir lätt att ersätta ORM:n utan att behöva ändra controllers funktioner, vilket i sin tur passar bra med open/closed principle för clean code: applikationen blir öppen för utvidgning men stängd för ändring.

Dock min motivation att välja just det här designmönstret ligger framför allt i att den gör controllern lätt att testa. Att kommunicera med databasen direkt från controllern innebär en hel del sidoeffekter i funktionerna. Att implementera Repository Pattern innebär helt enkelt att införa en lager mellan controllern och data access och flytta alla sidoeffekter till detta lagret. Controller kan lätt och enkelt, tack vara dependency injection, lätt och enkelt ersätta repositoryn med en mockobjekt för att ta bort sidoeffekter.

I min applikation har jag en `IIngredientRepository` samt dess implementation, `IngredientRepository` som separerar databashanteringen från controllern. I testprojektet använde jag mig av en `Mock<IIngredientRepository>` får att få de sidoeffekterna bort från koden.Hade jag utvecklat min applikation vidare och implementerat flera olika modellklasser för ingredienser istället för bara en, så skulle jag implementera en generisk `IRepository` och även Unit of Work designmönster. Unit of Work är en bra "tillägg" till Repository Pattern och gör det möjlig att hantera flera Repositories genom en klass.

Ett annat designmönster som hade varit bra att använda men som jag valde bort var Strategy Pattern. Den skulle vara bra för att hantera lagersaldoändringar. I så fall skulle jag ha en strategy interface och tre konkreta strategy klasser som skulle hantera: manuell ändring av lagersaldo, massleverans samt minskning av lagersaldo. Vilken strategy som skulle användas bestämms i runtime. Anledningen att välja bort det här designmönstret är att det kanske skulle vara lite overengineering för de tre enkla operationer samt att jag tänkte att en abstraktion genom Repository Pattern som underlättar testningen var viktigare.
