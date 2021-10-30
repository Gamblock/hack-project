mergeInto(LibraryManager.library, {
  SendEventForTokenMint: function () {
    window.parent.postMessage({modelId: "617c16c75e6fd84834dee4dd", printingEvent: true}, '*');
  },
  Hello: function () {
    window.alert("Hello, world!");
  },
});