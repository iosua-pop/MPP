using System;

namespace chat.network.dto
{
	
	[Serializable]
	public class MessageDTO
	{
		private string senderId;
		private string receiverId;
		private string text;

		public MessageDTO(string senderId, string text, string receiverId)
		{
			this.senderId = senderId;
			this.text = text;
			this.receiverId = receiverId;
		}

		public virtual string SenderId
		{
			get
			{
				return senderId;
			}
		}

		public virtual string ReceiverId
		{
			get
			{
				return receiverId;
			}
		}

		public virtual string Text
		{
			get
			{
				return text;
			}
		}
	}

}