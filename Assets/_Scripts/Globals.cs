using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Values {
    public static class Globals{

        public enum unitStates {
            idle,
            walking,
            attacking,
            harvesting
        };

        //Noms pour les unités
        private static string[] nameArray = {"Véronique",
                                            "Brandon",
                                            "Capucine",
                                            "Philibert",
                                            "Avenance",
                                            "Germaine",
                                            "Pépin",
                                            "Dassien",
                                            "Donatien",
                                            "Magnus",
                                            "Athanase",
                                            "Eugène",
                                            "Alberto",
                                            "Pépito",
                                            "Micheline",
                                            "Chafouin",
                                            "Jean-Célestin",
                                            "Jean-Eudes",
                                            "Clafouti",
                                            "Ronald",
                                            "Edouard",
                                            "Yves",
                                            "Geneviève",
                                            "Cunégonde",
                                            "Louise-Emilie Villeneuve de Châteauneuf",
                                            "Gérardo",
                                            "Chirac",
                                            "Arthur",
                                            "Léa",
                                            "Marie-Lou",
                                            "Vriytra",
                                            "Didier Starbucks",
                                            "Richard Moelleux",
                                            "Cassoulin",
                                            "Imogène",
                                            "Jeannine",
                                            "Yvette",
                                            "Lucienne",
                                            "Yvonne",
                                            "Raymonde",
                                            "Alphonse",
                                            "Henriette",
                                            "Berthe"};

        public static List<string> RANDOM_NAMES_LIST = new List<string>(nameArray);
    }
}
