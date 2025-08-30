# MEDVBiServ

**MEDVBiServ** ist eine selbst entwickelte Anwendung zur Verwaltung und Bereitstellung von Bibelversen in mehreren Sprachen.  
Das Projekt wurde im Rahmen meiner Ausbildung als Fachinformatiker für Anwendungsentwicklung entwickelt und dient als praktische Unterstützung für mein Kirchen-Media-Team sowie als Lern- und Übungsprojekt für moderne Softwarearchitekturen.

---

## 🚀 Features

- **Mehrsprachigkeit**: Bibelverse in **Deutsch** und **Französisch** (weitere Sprachen geplant).
- **Such- und Filterfunktion**: Schnelles Auffinden von Versen nach Buch, Kapitel, Versnummer oder Schlagwort.
- **REST-API** mit Paging: Strukturierte Datenabfragen über eine saubere Schnittstelle.
- **Swagger-Dokumentation**: API ist direkt über Swagger testbar und dokumentiert.
- **DTOs & Mapper**: Strikte Trennung zwischen Datenbankmodellen und Transportobjekten.
- **Clean Architecture**: Klare Aufteilung in **Domain**, **Application**, **Infrastructure** und **API**.
- **SQLite-Datenbanken**: Jeweils eine eigenständige Datenbank pro Sprache (Deutsch / Französisch).

---

## 🛠️ Tech Stack

- **C# / .NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQLite**
- **Swagger / OpenAPI**
