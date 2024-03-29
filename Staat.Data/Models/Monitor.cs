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
using System.Text.Json.Serialization;

namespace Staat.Data.Models
{
    public class Monitor : ITimeStampedModel
    {
        [Key] public int Id { get; set; }
        [JsonIgnore] public virtual int MonitorTypeId { get; set; }

        [EnumDataType(typeof(MonitorType)), Required]
        public MonitorType Type
        {
            get => (MonitorType) MonitorTypeId;
            set => MonitorTypeId = (int) value;
        }

        [Required] public string Host { get; set; }
        public int? Port { get; set; }
        public bool? ValidateSsl { get; set; }
        [Required, MaxLength(14), StringLength(14)] public string MonitorCron { get; set; }
        [Required] public DateTime NextRunTime { get; set; }
        [Required] public DateTime LastRunTime { get; set; }

        public Incident CurrentIncident { get; set; }
        [Required] public Service Service { get; set; }

        [UseFiltering, UseSorting] public ICollection<MonitorData> MonitorData { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}