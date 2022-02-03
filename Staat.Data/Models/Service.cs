/*
 * Staat - Staat
 * Copyright (C) 2021 Bijstaan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or (at your
 * option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.ComponentModel.DataAnnotations;

namespace Staat.Data.Models
{
    public class Service : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        
        [Required, MaxLength(100), StringLength(100)] public string Name { get; set; }

        [StringLength(255), MaxLength(255)] public string Description { get; set; }
        [Url] public string Url { get; set; }

        [Required] public Status Status { get; set; }

        [UseFiltering, UseSorting] public ICollection<Incident> Incidents { get; set; }
        [Required] public ServiceGroup Group { get; set; }
        public Service Parent { get; set; }

        [UseFiltering, UseSorting] public ICollection<Service> Children { get; set; }

        [UseFiltering, UseSorting] public ICollection<Monitor> Monitors { get; set; }

        [UseFiltering, UseSorting] public ICollection<Maintenance> Maintenance { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}