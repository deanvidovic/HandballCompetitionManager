(function () {
  const statNumbers = document.querySelectorAll("[data-stat-target]");

  if (statNumbers.length === 0) {
    return;
  }

  const animationDuration = 900;

  const formatValue = function (value, suffix) {
    return Math.round(value).toString() + suffix;
  };

  const animateNumber = function (element) {
    const target = Number(element.dataset.statTarget);
    const suffix = element.dataset.statSuffix || "";
    const startTime = performance.now();

    const update = function (currentTime) {
      const elapsed = currentTime - startTime;
      const progress = Math.min(elapsed / animationDuration, 1);
      const easedProgress = 1 - Math.pow(1 - progress, 3);

      element.textContent = formatValue(target * easedProgress, suffix);

      if (progress < 1) {
        requestAnimationFrame(update);
      }
    };

    requestAnimationFrame(update);
  };

  const startAnimations = function () {
    statNumbers.forEach(animateNumber);
  };

  if ("IntersectionObserver" in window) {
    const observer = new IntersectionObserver(function (entries) {
      if (entries.some(function (entry) { return entry.isIntersecting; })) {
        startAnimations();
        observer.disconnect();
      }
    }, { threshold: 0.25 });

    observer.observe(document.querySelector(".stats-section"));
    return;
  }

  startAnimations();
})();
