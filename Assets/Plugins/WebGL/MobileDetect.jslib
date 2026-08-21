mergeInto(LibraryManager.library, {
  IsMobileInput: function() {
    return window.matchMedia('(pointer: coarse)').matches ? 1 : 0;
  }
});
