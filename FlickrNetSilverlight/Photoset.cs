﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FlickrNet
{
    /// <summary>
    /// A set of properties for the photoset.
    /// </summary>
    public sealed class Photoset : IFlickrParsable
    {
        private Uri _url;

        /// <summary>
        /// The ID of the photoset.
        /// </summary>
        public string PhotosetId { get; private set; }

        /// <summary>
        /// The URL of the photoset.
        /// </summary>
        public Uri Url
        {
            get
            {
                if (_url == null) _url = new Uri(String.Format(System.Globalization.CultureInfo.InvariantCulture, "http://www.flickr.com/photos/{0}/sets/{1}/", OwnerId, PhotosetId));
                return _url;
            }
            private set { _url = value; }
        }

        /// <summary>
        /// The ID of the owner of the photoset.
        /// </summary>
        public string OwnerId { get; internal set; }

        /// <summary>
        /// The photo ID of the primary photo of the photoset.
        /// </summary>
        public string PrimaryPhotoId { get; private set; }

        /// <summary>
        /// The secret for the primary photo for the photoset.
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// The server for the primary photo for the photoset.
        /// </summary>
        public string Server { get; private set; }

        /// <summary>
        /// The server farm for the primary photo for the photoset.
        /// </summary>
        public string Farm { get; private set; }

        /// <summary>
        /// The number of photos in this photoset.
        /// </summary>
        public int NumberOfPhotos { get; private set; }

        /// <summary>
        /// The number of videos in this photoset.
        /// </summary>
        public int NumberOfVideos { get; private set; }

        /// <summary>
        /// The title of the photoset.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The description of the photoset.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// The URL for the thumbnail of a photo.
        /// </summary>
        public Uri PhotosetThumbnailUrl
        {
            get { return UtilityMethods.UrlFormat(this, "_t", "jpg"); }
        }

        /// <summary>
        /// The URL for the square thumbnail of a photo.
        /// </summary>
        public Uri PhotosetSquareThumbnailUrl
        {
            get { return UtilityMethods.UrlFormat(this, "_s", "jpg"); }
        }

        /// <summary>
        /// The URL for the small copy of a photo.
        /// </summary>
        public Uri PhotosetSmallUrl
        {
            get { return UtilityMethods.UrlFormat(this, "_m", "jpg"); }
        }

        void IFlickrParsable.Load(System.Xml.XmlReader reader)
        {
            if (reader.LocalName != "photoset")
                throw new ParsingException("Unknown element name '" + reader.LocalName + "' found in Flickr response");

            while (reader.MoveToNextAttribute())
            {
                switch (reader.LocalName)
                {
                    case "id":
                        PhotosetId = reader.Value;
                        break;
                    case "url":
                        Url = new Uri(reader.Value);
                        break;
                    case "owner_id":
                    case "owner":
                        OwnerId = reader.Value;
                        break;
                    case "primary":
                        PrimaryPhotoId = reader.Value;
                        break;
                    case "secret":
                        Secret = reader.Value;
                        break;
                    case "farm":
                        Farm = reader.Value;
                        break;
                    case "server":
                        Server = reader.Value;
                        break;
                    case "photos":
                    case "total":
                        NumberOfPhotos = int.Parse(reader.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        break;
                    case "videos":
                        NumberOfVideos = int.Parse(reader.Value, System.Globalization.NumberFormatInfo.InvariantInfo);
                        break;
                    default:
                        throw new ParsingException("Unknown attribute value: " + reader.LocalName + "=" + reader.Value);
                }
            }

            if (!reader.IsEmptyElement)
            {
                reader.Read();

                while (true)
                {
                    if (reader.Name == "title")
                    {
                        Title = reader.ReadInnerXml();
                        continue;
                    }
                    if (reader.Name == "description")
                    {
                        Description = reader.ReadInnerXml();
                        continue;
                    }
                    if (reader.NodeType == System.Xml.XmlNodeType.EndElement && reader.Name == "photoset")
                    {
                        reader.Read();
                    }
                    break;
                }

            }

            reader.Skip();
        }

    }
}
