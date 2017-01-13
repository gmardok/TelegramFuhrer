using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeleSharp.TL;

namespace TelegramFuhrer.Data.Entities
{
	public class User : TLUser
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }

		[Required]
		public bool IsGlobalAdmin { get; set; }

		public ICollection<UserChat> UserChats { get; set; }

		public User()
		{
		}

		public User(TLUser tlUser)
		{
			Copy(tlUser);
		}

		public void Copy(TLUser tlUser)
		{
			id = tlUser.id;
			username = tlUser.username;
			flags = tlUser.flags;
			self = tlUser.self;
			contact = tlUser.contact;
			mutual_contact = tlUser.mutual_contact;
			deleted = tlUser.deleted;
			bot = tlUser.bot;
			bot_chat_history = tlUser.bot_chat_history;
			bot_nochats = tlUser.bot_nochats;
			verified = tlUser.verified;
			restricted = tlUser.restricted;
			min = tlUser.min;
			bot_inline_geo = tlUser.bot_inline_geo;
			access_hash = tlUser.access_hash;
			first_name = tlUser.first_name;
			last_name = tlUser.last_name;
			phone = tlUser.phone;
			photo = tlUser.photo;
			status = tlUser.status;
			bot_info_version = tlUser.bot_info_version;
			restriction_reason = tlUser.restriction_reason;
			bot_inline_placeholder = tlUser.bot_inline_placeholder;
		}

		public bool IsSame(TLUser tlUser)
		{
			return username == tlUser.username
			       && flags == tlUser.flags
			       && self == tlUser.self
			       && contact == tlUser.contact
			       && mutual_contact == tlUser.mutual_contact
			       && deleted == tlUser.deleted
			       && bot == tlUser.bot
			       && bot_chat_history == tlUser.bot_chat_history
			       && bot_nochats == tlUser.bot_nochats
			       && verified == tlUser.verified
			       && restricted == tlUser.restricted
			       && min == tlUser.min
			       && bot_inline_geo == tlUser.bot_inline_geo
			       && access_hash == tlUser.access_hash
			       && first_name == tlUser.first_name
			       && last_name == tlUser.last_name
			       && phone == tlUser.phone
			       && bot_info_version == tlUser.bot_info_version
			       && restriction_reason == tlUser.restriction_reason
			       && bot_inline_placeholder == tlUser.bot_inline_placeholder;
		}

	}
}
