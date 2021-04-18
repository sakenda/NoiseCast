using System;
using System.Collections.ObjectModel;

namespace WPFMVVM.MVVM.Core
{
    internal static class MockData
    {
        public static ObservableCollection<Podcast> GetPodcastMockData()
        {
            ObservableCollection<Podcast> result = new ObservableCollection<Podcast>();
            ObservableCollection<Episode> episode;

            episode = new ObservableCollection<Episode>()
                {
                    new Episode(
                    "Hooked FM #316 - Monster Hunter Rise, Outriders, E3 2021, The Last of Us-Remake & mehr!",
                    new DateTime(2021, 4, 12),
                    "Wir reden ausführlich über aktuelle Ereignisse bei Sony, die kommende E3 2021 und unsere Eindrücke zu Monster Hunter Rise und Outriders - das alles und mehr jetzt bei Folge 316 von Hooked FM!",
                    "https://cdn.podigee.com/media/podcast_6853_stay_forever_episode_393250_die_stay_forever_challenge_2_game_not_over.mp3?v=1614509959&amp;source=feed"
                    ),
                    new Episode(
                    "Hooked FM #315 - Outriders, Evil Genius 2, Binding of Isaac: Repentance, Disco Elysium: The Final Cut & mehr!",
                    new DateTime(2021, 4, 5),
                    "Leo ist zu Gast und redet mit uns ausführlich über Evil Genius 2, Outriders und den Final Cut von Disco Elysium! Später kommt noch Lucy dazu, um mit Robin über The Binding of Isaac: Repentance zu quatschen. Viel Spaß mit Folge 315 von Hooked FM in Überlänge!",
                    "https://cdn.podigee.com/media/podcast_6853_stay_forever_episode_393250_die_stay_forever_challenge_2_game_not_over.mp3?v=1614509959&amp;source=feed"
                    )
                };
            result.Add(new Podcast(
                "Hooked",
                "Hooked",
                "/Images/hooked.jpg",
                @"http://www.hooked.de",
                episode,
                new DateTime(2021, 1, 23),
                "Jede Woche Montag erscheint mit Hooked FM unser Podcast rund um das Thema Videospiele.Und manchmal auch Serien.Und Filme.Und überhaupt."
                ));

            episode = new ObservableCollection<Episode>()
            {
                new Episode(
                    "Mietendeckel gekippt, K-Frage, Bundes-Notbremse, überschätzte Antigen-Schnelltests, Ukraine vs. Russland, Afghanistan-Rückzug",
                    new DateTime(2021, 4, 15),
                    @"Begrüßung

                    Website Lage Plus
                    Gekippter Berliner Mietendeckel

                    „Keine Mieterin und kein Mieter der Deutsche Wohnen wird durch die Entscheidung die Wohnung verlieren“ (deutsche-wohnen.com)
                    Mietendeckel verfassungswidrig – Verfassung geht vor Ideologie (CDU/CSU-Fraktion)
                    Vonovia verzichtet auf Mietnachzahlungen (https://presse.vonovia.de)
                    Presse: Gesetz zur Mietenbegrenzung im Wohnungswesen in Berlin („Berliner Mietendeckel“) nichtig (bundesverfassungsgericht.de)
                    Entscheidungen: Gesetz zur Mietenbegrenzung im Wohnungswesen in Berlin („Berliner Mietendeckel“) nichtig (bundesverfassungsgericht.de)
                    § 543 BGB – Einzelnorm (gesetze-im-internet.de)
                    Kevin Kühnert fordert bundesweiten Mietendeckel (tagesspiegel.de)
                    K-Frage in der Union

                    Doppelter Umfrage-Tiefschlag im Kanzler-Showdown: Laschet gleich zweimal abgestraft – Söder an der Spitze (https://www.merkur.de)
                    „Die CDU soll in sich gehen“ (Süddeutsche.de)
                     Vom Fahrrad-Lobbyisten zum CDU-Mann: Der Abtrünnige (spiegel.de)
                    Lächelnd im Hagelsturm (Süddeutsche.de)
                    Suche nach dem Kanzlerkandidaten: Hauen nach Zahlen (Süddeutsche.de)
                    Armin Laschet und Markus Söder in der CDU/CSU-Fraktion: Die Entladung (spiegel.de)
                    Kanzlerkandidatur: Was Söder antreibt (Süddeutsche.de)
                    CDU: Laschet bremst Söder aus (Süddeutsche.de)
                    K-Frage spaltet die Union (Süddeutsche.de)
                    CDU: Abgeordneter zieht sich zurück (Süddeutsche.de)
                    Kanzlerkandidatur: Söder oder Laschet (Süddeutsche.de)
                    Rangeleien beim Schlussspurt (Süddeutsche.de)
                    Kanzlerkandidatur: Söder ist bereit (Süddeutsche.de)
                    Corona: Bundes-Notbremse

                    Verhängung einer Ausgangsbeschränkung während der Corona-Pandemie  (rechtsprechung.niedersachsen.de)
                    Epidemiologe kritisiert Orientierung an Sieben-Tage-Inzidenz (tagesschau.de)
                    Infektionsschutz: Jetzt kommt das Bundesverfassungsgericht ins Spiel (Süddeutsche.de)
                    Johnson&Johnson stoppt Auslieferung nach Europa (Süddeutsche.de)
                    Corona-Zahlen: Zahlen und Grafiken zum Coronavirus (Süddeutsche.de)
                    Bundestag entscheidet nächsten Mittwoch (Süddeutsche.de)
                    Corona: Die Schulen haben keine faire Chance (Süddeutsche.de)
                    Digitaler Impfpass soll noch vor den Sommerferien kommen (badische-zeitung.de)
                    Corona-Impfung: Biontech liefert an EU weitere 1,8 Milliarden Dosen (Süddeutsche.de)
                    Bundesweite Corona-Notbremse kommt erst Ende nächster Woche – das ist nun geplant (tagesspiegel.de)
                    Drosten und Lauterbach reicht die Notbremse nicht aus (tagesspiegel.de)
                    Sascha Lobo: Warum ich meine eigenen Coronaregeln mache (spiegel.de)
                    He started a covid-19 vaccine company. Then he hosted a superspreader event. (MIT Technology Review)
                    A false sense of security… (diamandis.com)",
                    "https://cdn.podigee.com/media/podcast_6853_stay_forever_episode_393250_die_stay_forever_challenge_2_game_not_over.mp3?v=1614509959&amp;source=feed"
                    ),
                new Episode(
                    "Spahns Spenden-Dinner, Ukraine-Konflikt, Brückenlockdown und Pilotprojekte, K-Frage in der Union, digitaler Impfausweis",
                    new DateTime(2021, 4, 8),
                    @"Begrüßung

                    Website Lage Plus
                    Spenden-Dinner des Gesundheitsministers

                    Spendendinner des Gesundheitsministers: Spahns Schweigekartell (taz.de)
                    Kampagnenleiter von LobbyControl: „Das ist ein politisches No-Go“ (taz.de)",
                    "https://cdn.podigee.com/media/podcast_6853_stay_forever_episode_393250_die_stay_forever_challenge_2_game_not_over.mp3?v=1614509959&amp;source=feed"
                    )
            };
            result.Add(new Podcast(
                "Lage der Nation",
                "kuechenstud.io",
                "/Images/lage.png",
                "www.lagedernation.de",
                episode,
                new DateTime(2021, 3, 12),
                "Jede Woche lassen Philip Banse und Ulf Buermeyer die politische Woche Revue passieren und kommentieren die Lage der Nation."));

            episode = new ObservableCollection<Episode>()
            {
                new Episode(
                    "Anstoss (SF #108)",
                    new DateTime(2021, 4, 18),
                    @"Aus dem Hintergrund müsste Schmidt schießen!
                    Ende der 80er schrieb ein junger Fußballfan zum eigenen Spaß ein Programm, um den Ausgang der Elferwette zu simulieren. Das kleine Tool wuchs und wuchs und wurde schließlich zu einem halbprofessionellen Fußballmanager – den der Entwickler, Gerald Köhler, dann im Eigenvertrieb an den Mann brachte. Der Publisher Ascon wurde aufmerksam auf das Projekt, machte ein Angebot, polierte das Spiel nochmal ordentlich auf und der Rest ist Geschichte.
                    Das Spiel erschien Ende 1993 als Anstoss, war ein enormer Erfolg und legte den Grundstein für eine kleine Serie von vier Teilen und zwei Spin-Offs. Bisher jedenfalls, aber mit Anstoss 2022 ist, knapp 20 Jahre nach dem bisher letzten Teil Anstoss 4 wieder ein Anstoss geplant. Zudem arbeitet Gerald Köhler himself an We are Football, einem spirituellen Nachfolger.
                    Gunnar und Christian sprechen über die Entstehung von Teil 1, die Entwicklung der Serie und lassen auch den Erfinder von Anstoss, Gerald Köhler, ausführlich zu Wort kommen.",
                    "https://cdn.podigee.com/media/podcast_6853_stay_forever_episode_393250_die_stay_forever_challenge_2_game_not_over.mp3?v=1614509959&amp;source=feed"
                    ),
                new Episode(
                    "Sinclair ZX81 (Stay Forever Technik #2)",
                    new DateTime(2021, 4, 11),
                    @"Ein Computer-Bausatz erobert Großbritannien!
                    Stay Forever Technik ist unsere Ergänzung zu den auf Spiele fokussierten Formaten Stay Forever und Super Stay Forever und beschäftigt sich mit der Technik rund um das Spielen am Computer oder der Konsole.
                    Folge 1 drehte sich um Soundkarten am PC, in Folge 2 begeben sich Henner Thomsen und Gunnar Lott auf eine Reise in die Zeit vor der Hegemonie der DOS-PCs – und sprechen über die britischen Selbstbau-Computer ZX80 und ZX81 der Firma Sinclair. Die günstigen (wenn auch sehr eingeschränkten) Geräte legten, gemeinsam mit ihrem Nachfolger, dem ZX Spectrum, die Grundlage für die britische Spielebranche, denn der ZX81 war eine frühe Spielemaschine, auf der viele spätere Helden des Gaming ihre ersten Versuche starteten, von David Perry (dem späteren Macher von Earthworm Jim) über Jeff Minter (Revenge of the Mutant Camels) bis zu den Stamper Brothers (Gründer von Rare).
                    Gunnar und Henner zeichnen die Geschichte des ZX81 nach (inklusive der Historie der Firma Sinclair), stellen die Technik vor und erklären seinen Erfolg (und seinen Untergang).
                    Hinweis: Die Folge hat umfangreiche Kapitelmarken, in denen ihr Bilder zu den Geräten und Spielen sehen könnt, die wir besprechen. Ihr braucht aber einen Podcatcher, der die anzeigen kann, wir empfehlen Podcast Addict, Pocket Casts und Overcast.",
                    "https://cdn.podigee.com/media/podcast_6853_stay_forever_episode_393250_die_stay_forever_challenge_2_game_not_over.mp3?v=1614509959&amp;source=feed"
                    )
            };
            result.Add(new Podcast(
                "Stay Forever",
                "Stay Forever GmbH",
                "/Images/stay.jpg",
                @"http://www.stayforever.de",
                episode,
                new DateTime(2021, 04, 18),
                "Ein Podcast über die aufregenden Games der 80er und 90er Jahre. Begleitet uns auf archäologische Expeditionen und pustet mit uns vorsichtig den Staub der Jahre von wunderbaren alten PC- und Videospielen. Es sprechen, in wechselnden Konstellationen, Gunnar Lott, Christian Schmidt und Fabian Käufer."));

            return result;
        }
    }
}