(() => {
  const reduce = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
  const nav = document.querySelector("[data-nav]");
  const toggle = document.querySelector("[data-nav-toggle]");
  const progress = document.querySelector("[data-progress]");
  const cursor = document.querySelector("[data-cursor]");
  const canvas = document.querySelector("[data-particles]");
  const links = nav?.querySelectorAll("a[href^='#']") ?? [];

  const onScroll = () => {
    if (nav) {
      nav.classList.toggle("is-scrolled", window.scrollY > 12);
    }
    if (progress) {
      const max = document.documentElement.scrollHeight - window.innerHeight;
      const ratio = max > 0 ? window.scrollY / max : 0;
      progress.style.width = `${Math.min(100, Math.max(0, ratio * 100))}%`;
    }
  };

  window.addEventListener("scroll", onScroll, { passive: true });
  onScroll();

  toggle?.addEventListener("click", () => {
    const open = nav?.classList.toggle("is-open");
    toggle.setAttribute("aria-expanded", open ? "true" : "false");
  });

  links.forEach((link) => {
    link.addEventListener("click", () => {
      nav?.classList.remove("is-open");
      toggle?.setAttribute("aria-expanded", "false");
    });
  });

  // Split brand letters
  document.querySelectorAll("[data-split]").forEach((el) => {
    const text = el.textContent ?? "";
    el.textContent = "";
    [...text].forEach((ch, i) => {
      const span = document.createElement("span");
      span.className = "char";
      span.textContent = ch === " " ? "\u00A0" : ch;
      span.style.animationDelay = `${0.04 * i + 0.15}s`;
      el.appendChild(span);
    });
  });

  // Typewriter headline
  const typeEl = document.querySelector("[data-type]");
  if (typeEl && !reduce) {
    const words = (typeEl.getAttribute("words") || "")
      .split("|")
      .map((w) => w.trim())
      .filter(Boolean);
    let wordIndex = 0;
    let charIndex = 0;
    let deleting = false;

    const tick = () => {
      const current = words[wordIndex] || "";
      typeEl.textContent = current.slice(0, charIndex);
      if (!deleting && charIndex < current.length) {
        charIndex += 1;
        setTimeout(tick, 42);
        return;
      }
      if (!deleting && charIndex === current.length) {
        deleting = true;
        setTimeout(tick, 1400);
        return;
      }
      if (deleting && charIndex > 0) {
        charIndex -= 1;
        setTimeout(tick, 24);
        return;
      }
      deleting = false;
      wordIndex = (wordIndex + 1) % words.length;
      setTimeout(tick, 280);
    };
    tick();
  } else if (typeEl) {
    const first = (typeEl.getAttribute("words") || "").split("|")[0] || "";
    typeEl.textContent = first;
  }

  // Reveal on scroll
  const reveals = document.querySelectorAll("[data-reveal]");
  if (!("IntersectionObserver" in window) || reduce) {
    reveals.forEach((el) => el.classList.add("is-visible"));
  } else {
    const observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (!entry.isIntersecting) return;
          entry.target.classList.add("is-visible");
          observer.unobserve(entry.target);
        });
      },
      { rootMargin: "0px 0px -10% 0px", threshold: 0.12 }
    );
    reveals.forEach((el, i) => {
      el.style.transitionDelay = `${Math.min(i % 6, 5) * 0.07}s`;
      observer.observe(el);
    });
  }

  // Count-up stats
  const counters = document.querySelectorAll("[data-count]");
  const animateCount = (el) => {
    const target = Number(el.getAttribute("data-count") || "0");
    if (reduce) {
      el.textContent = String(target);
      return;
    }
    const start = performance.now();
    const duration = 1200;
    const step = (now) => {
      const t = Math.min(1, (now - start) / duration);
      const eased = 1 - Math.pow(1 - t, 3);
      el.textContent = String(Math.round(target * eased));
      if (t < 1) requestAnimationFrame(step);
    };
    requestAnimationFrame(step);
  };

  if ("IntersectionObserver" in window) {
    const countObserver = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (!entry.isIntersecting) return;
          animateCount(entry.target);
          countObserver.unobserve(entry.target);
        });
      },
      { threshold: 0.5 }
    );
    counters.forEach((el) => countObserver.observe(el));
  } else {
    counters.forEach(animateCount);
  }

  // Magnetic buttons
  if (!reduce) {
    document.querySelectorAll("[data-magnetic]").forEach((btn) => {
      btn.addEventListener("mousemove", (e) => {
        const rect = btn.getBoundingClientRect();
        const x = e.clientX - rect.left - rect.width / 2;
        const y = e.clientY - rect.top - rect.height / 2;
        btn.style.transform = `translate(${x * 0.18}px, ${y * 0.22}px)`;
      });
      btn.addEventListener("mouseleave", () => {
        btn.style.transform = "";
      });
    });
  }

  // Card tilt
  if (!reduce) {
    document.querySelectorAll("[data-tilt]").forEach((card) => {
      card.addEventListener("mousemove", (e) => {
        const rect = card.getBoundingClientRect();
        const px = (e.clientX - rect.left) / rect.width;
        const py = (e.clientY - rect.top) / rect.height;
        const rx = (0.5 - py) * 10;
        const ry = (px - 0.5) * 12;
        card.style.transform = `perspective(900px) rotateX(${rx}deg) rotateY(${ry}deg) translateY(-4px)`;
      });
      card.addEventListener("mouseleave", () => {
        card.style.transform = "";
      });
    });
  }

  // Soft cursor glow
  if (cursor && !reduce && window.matchMedia("(pointer: fine)").matches) {
    let mx = -9999;
    let my = -9999;
    let cx = mx;
    let cy = my;
    window.addEventListener(
      "pointermove",
      (e) => {
        mx = e.clientX;
        my = e.clientY;
        cursor.classList.add("is-on");
      },
      { passive: true }
    );
    const loopCursor = () => {
      cx += (mx - cx) * 0.12;
      cy += (my - cy) * 0.12;
      cursor.style.transform = `translate3d(${cx}px, ${cy}px, 0)`;
      requestAnimationFrame(loopCursor);
    };
    requestAnimationFrame(loopCursor);
  }

  // Particles
  if (canvas instanceof HTMLCanvasElement && !reduce) {
    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    let w = 0;
    let h = 0;
    let particles = [];

    const resize = () => {
      w = canvas.width = window.innerWidth;
      h = canvas.height = window.innerHeight;
      const count = Math.min(70, Math.floor((w * h) / 28000));
      particles = Array.from({ length: count }, () => ({
        x: Math.random() * w,
        y: Math.random() * h,
        r: Math.random() * 1.6 + 0.4,
        vx: (Math.random() - 0.5) * 0.25,
        vy: (Math.random() - 0.5) * 0.25,
        a: Math.random() * 0.35 + 0.1
      }));
    };

    const draw = () => {
      ctx.clearRect(0, 0, w, h);
      for (const p of particles) {
        p.x += p.vx;
        p.y += p.vy;
        if (p.x < 0) p.x = w;
        if (p.x > w) p.x = 0;
        if (p.y < 0) p.y = h;
        if (p.y > h) p.y = 0;
        ctx.beginPath();
        ctx.arc(p.x, p.y, p.r, 0, Math.PI * 2);
        ctx.fillStyle = `rgba(61, 255, 224, ${p.a})`;
        ctx.fill();
      }
      for (let i = 0; i < particles.length; i += 1) {
        for (let j = i + 1; j < particles.length; j += 1) {
          const a = particles[i];
          const b = particles[j];
          const dx = a.x - b.x;
          const dy = a.y - b.y;
          const dist = Math.hypot(dx, dy);
          if (dist < 110) {
            ctx.strokeStyle = `rgba(61, 255, 224, ${0.08 * (1 - dist / 110)})`;
            ctx.beginPath();
            ctx.moveTo(a.x, a.y);
            ctx.lineTo(b.x, b.y);
            ctx.stroke();
          }
        }
      }
      requestAnimationFrame(draw);
    };

    window.addEventListener("resize", resize, { passive: true });
    resize();
    draw();
  }
})();
