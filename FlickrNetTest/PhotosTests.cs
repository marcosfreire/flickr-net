﻿using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosTests
    /// </summary>
    [TestClass]
    public class PhotosTests
    {
        public PhotosTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void PhotosGetAllContextsBasicTest()
        {
            var a = TestData.GetInstance().PhotosGetAllContexts("4114887196");

            Assert.IsNotNull(a);
            Assert.IsNotNull(a.Groups, "Groups should not be null.");
            Assert.IsNotNull(a.Sets, "Sets should not be null.");

            Assert.AreEqual(1, a.Groups.Count, "Groups.Count should be one.");
            Assert.AreEqual(1, a.Sets.Count, "Sets.Count should be one.");
        }

        [TestMethod]
        public void PhotosGetExifTest()
        {
            Flickr f = TestData.GetInstance();

            ExifTagCollection tags = f.PhotosGetExif("4268023123");

            Assert.IsNotNull(tags, "ExifTagCollection should not be null.");

            Assert.IsTrue(tags.Count > 20, "More than twenty parts of EXIF data should be returned.");

            Assert.AreEqual("System", tags[0].TagSpace, "First tags TagSpace is not set correctly.");
            Assert.AreEqual(0, tags[0].TagSpaceId, "First tags TagSpaceId is not set correctly.");
            Assert.AreEqual("FileName", tags[0].Tag, "First tags Tag is not set correctly.");
            Assert.AreEqual("FileName", tags[0].Label, "First tags Label is not set correctly.");
            Assert.AreEqual("ORI46620478284895704.img", tags[0].Raw, "First tags RAW is not correct.");
            Assert.IsNull(tags[0].Clean, "First tags Clean should be null.");
        }

        [TestMethod]
        public void PhotosGetPermsBasicTest()
        {
            var p = TestData.GetAuthInstance().PhotosGetPerms("4114887196");

            Assert.IsNotNull(p);
            Assert.AreEqual("4114887196", p.PhotoId);
            Assert.AreNotEqual(PermissionComment.Nobody, p.PermissionComment);
        }

        [TestMethod]
        public void PhotosGetSizesBasicTest()
        {
            var sizes = TestData.GetInstance().PhotosGetSizes("4114887196");

            Assert.IsNotNull(sizes);
            Assert.AreNotEqual(0, sizes.Count);

            foreach (Size s in sizes)
            {
                Assert.IsNotNull(s.Label, "Label should not be null.");
                Assert.IsNotNull(s.Source, "Source should not be null.");
                Assert.IsNotNull(s.Url, "Url should not be null.");
                Assert.AreNotEqual(0, s.Height, "Height should not be zero.");
                Assert.AreNotEqual(0, s.Width, "Width should not be zero.");
                Assert.AreNotEqual(MediaType.None, s.MediaType, "MediaType should be set.");
            }
        }

        [TestMethod]
        public void PhotosGetSizesVideoTest()
        {
            //http://www.flickr.com/photos/tedsherarts/4399135415/
            var sizes = TestData.GetInstance().PhotosGetSizes("4399135415");

            bool findVideo = false;
            bool findPhoto = false;
            foreach (var s in sizes)
            {
                if (s.MediaType == MediaType.Videos) findVideo = true;
                if (s.MediaType == MediaType.Photos) findPhoto = true;
            }
            Assert.IsTrue(findVideo, "At least one size should contain a Video media type.");
            Assert.IsTrue(findPhoto, "At least one size should contain a Photo media type.");
        }

        [TestMethod]
        public void PhotosGetContextBasicTest()
        {
            var context = TestData.GetInstance().PhotosGetContext("4114887196");

            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void PhotosGetSizes50Test()
        {
            Flickr.FlushCache();

            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Tags = "microsoft";
            o.PerPage = 50;

            PhotoCollection photos = TestData.GetInstance().PhotosSearch(o);

            foreach (Photo p in photos)
            {
                var sizes = TestData.GetInstance().PhotosGetSizes(p.PhotoId);
                foreach (var s in sizes)
                {

                }
            }
        }

        [TestMethod]
        public void PhotosSearchDoesLargeExist()
        {
            PhotoSearchOptions o = new PhotoSearchOptions();
            o.Extras = PhotoSearchExtras.AllUrls;
            o.PerPage = 50;
            o.Tags = "test";

            PhotoCollection photos = TestData.GetInstance().PhotosSearch(o);

            foreach (Photo p in photos)
            {
                if (p.DoesLargeExist)
                    Console.WriteLine(p.LargeUrl);
                else
                    Console.WriteLine("No Large Url");
                if (p.DoesMediumExist)
                    Console.WriteLine(p.MediumUrl);
                else
                    Console.WriteLine("No Medium Url");
            }
        }

        [TestMethod]
        public void PhotosUploadCheckTicketsTest()
        {
            Flickr f = TestData.GetInstance();

            string[] tickets = new string[3];
            tickets[0] = "invalid1";
            tickets[1] = "invalid2";
            tickets[2] = "invalid3";

            var t = f.PhotosUploadCheckTickets(tickets);

            Assert.AreEqual(3, t.Count);

            Assert.AreEqual("invalid1", t[0].TicketId);
            Assert.IsNull(t[0].PhotoId);
            Assert.IsTrue(t[0].InvalidTicketId);

        }
    }
}