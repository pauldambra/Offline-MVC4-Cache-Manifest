using System.Collections.Generic;
using OfflineAppCache.Models;

namespace OfflineAppCache.Data
{
    public class SuperHugeDatabase
    {
        public IEnumerable<Ship> Ships = new List<Ship>
            {
                new Ship("Different Tan", "Mountain class GCU"),
                new Ship("Fate Amenable To Change", "River class GCU"),
                new Ship("Grey Area", "GCU"),
                new Ship("It's Character Forming", "GCU"),
                new Ship("Jaundiced Outlook", "Ridge class GCU"),
                new Ship("Problem Child", "Troubadour class GCU"),
                new Ship("Reasonable Excuse", "GCU"),
                new Ship("Recent Convert", "GCU"),
                new Ship("Tactical Grace", "Escarpment class GCU"),
                new Ship("Unacceptable Behaviour", "GCU"),
                new Ship("Steely Glint", "Plains class GCV"),
                new Ship("Highpoint", "ulterior"),
                new Ship("Shoot Them Later", "eccentric; ulterior"),
                new Ship("Attitude Adjuster", "Killer class LOU"),
                new Ship("Killing Time", "Torturer class ROU"),
                new Ship("Frank Exchange Of Views", "Psychopath class dROU"),
                new Ship("Anticipation Of A New Lover's Arrival, The", "Plate class GSV"),
                new Ship("Death and Gravity", "GSV"),
                new Ship("Ethics Gradient", "Range class GSV"),
                new Ship("Honest Mistake", "GSV"),
                new Ship("Limivourous", "Ocean class GSV"),
                new Ship("No Fixed Abode", "Sabbaticaler class GSV, ex-Equator class"),
                new Ship("Quietly Confident", "Plate class GSV"),
                new Ship("Sleeper Service", "Plate class GSV"),
                new Ship("Uninvited Guest", "GSV"),
                new Ship("Use Psychology", "GSV"),
                new Ship("What Is The Answer and Why?", "GSV"),
                new Ship("Wisdom Like Silence", "Continent class GSV"),
                new Ship("Yawning Angel", "Range class GSV"),
                new Ship("Zero Gravitas", "GSV"),
                new Ship("Misophist", "LSV"),
                new Ship("Serious Callers Only", "Tundra class LSV"),
                new Ship("Not Invented Here", "Desert class MSV"),
            };
    }
}