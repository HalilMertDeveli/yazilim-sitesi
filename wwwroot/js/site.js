// Halil Mert Develi — portfolio v2 interactions.
// Small, dependency-free, and every effect degrades to a static page.
(() => {
  "use strict";

  const root = document.documentElement;
  const reduceMotion = window.matchMedia("(prefers-reduced-motion: reduce)");
  const finePointer = window.matchMedia("(hover: hover) and (pointer: fine)");

  /* ---------- Navigation ---------- */
  const nav = document.querySelector("[data-nav]");
  const toggle = document.querySelector("[data-nav-toggle]");
  const menu = document.querySelector("[data-nav-menu]");

  if (nav) {
    const onScroll = () => nav.classList.toggle("is-scrolled", window.scrollY > 12);
    onScroll();
    window.addEventListener("scroll", onScroll, { passive: true });
  }

  if (nav && toggle && menu) {
    const label = toggle.querySelector(".sr-only");
    const setOpen = (open) => {
      nav.classList.toggle("is-open", open);
      toggle.setAttribute("aria-expanded", String(open));
      document.body.style.overflow = open ? "hidden" : "";
      if (label) label.textContent = open ? label.dataset.labelClose : label.dataset.labelOpen;
    };

    toggle.addEventListener("click", () => setOpen(!nav.classList.contains("is-open")));
    menu.querySelectorAll("a").forEach((link) => link.addEventListener("click", () => setOpen(false)));
    document.addEventListener("keydown", (e) => {
      if (e.key === "Escape" && nav.classList.contains("is-open")) {
        setOpen(false);
        toggle.focus();
      }
    });
    window.matchMedia("(min-width: 961px)").addEventListener("change", (e) => {
      if (e.matches) setOpen(false);
    });
  }

  /* ---------- Current section in nav ---------- */
  const navLinks = [...document.querySelectorAll("[data-nav-link]")];
  const sections = navLinks
    .map((link) => document.querySelector(link.getAttribute("href")))
    .filter(Boolean);

  if ("IntersectionObserver" in window && sections.length) {
    const spy = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (!entry.isIntersecting) return;
          navLinks.forEach((link) => {
            const active = link.getAttribute("href") === `#${entry.target.id}`;
            link.classList.toggle("is-current", active);
            if (active) link.setAttribute("aria-current", "location");
            else link.removeAttribute("aria-current");
          });
        });
      },
      { rootMargin: "-45% 0px -50% 0px" }
    );
    sections.forEach((s) => spy.observe(s));
  }

  /* ---------- Reveal on scroll ---------- */
  const revealables = document.querySelectorAll("main [data-reveal]:not(.hero [data-reveal])");
  if ("IntersectionObserver" in window && !reduceMotion.matches) {
    const io = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (!entry.isIntersecting) return;
          entry.target.classList.add("is-visible");
          entry.target.querySelectorAll(".viz").forEach((v) => v.classList.add("is-visible"));
          io.unobserve(entry.target);
        });
      },
      { rootMargin: "0px 0px -8% 0px", threshold: 0.08 }
    );

    // Stagger siblings in grids so cards arrive in sequence.
    revealables.forEach((el) => {
      const siblings = el.parentElement ? [...el.parentElement.children].filter((c) => c.hasAttribute("data-reveal")) : [];
      const index = siblings.indexOf(el);
      if (index > 0) el.style.setProperty("--delay", `${Math.min(index, 5) * 70}ms`);
      io.observe(el);
    });
  } else {
    revealables.forEach((el) => el.classList.add("is-visible"));
    document.querySelectorAll(".viz").forEach((v) => v.classList.add("is-visible"));
  }

  /* ---------- Subtle depth on cards (fine pointers only) ---------- */
  if (finePointer.matches && !reduceMotion.matches) {
    document.querySelectorAll("[data-tilt]").forEach((el) => {
      const max = el.classList.contains("project") ? 2.5 : 3.5;
      let frame = 0;
      el.addEventListener("pointermove", (e) => {
        cancelAnimationFrame(frame);
        frame = requestAnimationFrame(() => {
          const r = el.getBoundingClientRect();
          const x = (e.clientX - r.left) / r.width - 0.5;
          const y = (e.clientY - r.top) / r.height - 0.5;
          el.style.setProperty("--ry", `${(x * max).toFixed(2)}deg`);
          el.style.setProperty("--rx", `${(-y * max).toFixed(2)}deg`);
        });
      });
      el.addEventListener("pointerleave", () => {
        cancelAnimationFrame(frame);
        el.style.setProperty("--ry", "0deg");
        el.style.setProperty("--rx", "0deg");
      });
    });
  }

  /* ---------- Copy email ---------- */
  const status = document.querySelector("[data-copy-status]");
  document.querySelectorAll("[data-copy]").forEach((btn) => {
    btn.addEventListener("click", async () => {
      const value = btn.getAttribute("data-copy");
      const label = btn.querySelector("[data-copy-label]");
      const original = label ? label.textContent : "";
      try {
        await navigator.clipboard.writeText(value);
        if (label) label.textContent = btn.dataset.copiedLabel;
        if (status) status.textContent = btn.dataset.copiedLabel;
        setTimeout(() => {
          if (label) label.textContent = original;
        }, 2000);
      } catch {
        window.location.href = `mailto:${value}`;
      }
    });
  });

  /* ---------- LED matrix background ---------- */
  // A dot grid that reads like an LED panel. Dots near the pointer light up,
  // and a slow diagonal wave passes through. Paused off-screen and when hidden.
  const canvas = document.querySelector("[data-matrix]");
  if (canvas && canvas.getContext) {
    const ctx = canvas.getContext("2d", { alpha: true });
    const hero = canvas.parentElement;
    const gap = 22;
    let w = 0, h = 0, dpr = 1, cols = 0, rows = 0;
    let pointer = { x: -9999, y: -9999, tx: -9999, ty: -9999 };
    let running = false, visible = true, raf = 0, start = performance.now();

    const resize = () => {
      const rect = hero.getBoundingClientRect();
      dpr = Math.min(window.devicePixelRatio || 1, 2);
      w = rect.width;
      h = rect.height;
      canvas.width = Math.round(w * dpr);
      canvas.height = Math.round(h * dpr);
      ctx.setTransform(dpr, 0, 0, dpr, 0, 0);
      cols = Math.ceil(w / gap) + 1;
      rows = Math.ceil(h / gap) + 1;
      draw(performance.now());
    };

    const draw = (now) => {
      const t = (now - start) / 1000;
      ctx.clearRect(0, 0, w, h);
      pointer.x += (pointer.tx - pointer.x) * 0.12;
      pointer.y += (pointer.ty - pointer.y) * 0.12;
      const animate = !reduceMotion.matches;

      for (let i = 0; i < cols; i++) {
        const x = i * gap + 4;
        for (let j = 0; j < rows; j++) {
          const y = j * gap + 4;
          let glow = 0;
          if (animate) {
            const wave = Math.sin((x * 0.6 + y) * 0.012 - t * 0.9);
            glow = Math.max(0, wave - 0.82) * 1.6;
            const dx = x - pointer.x;
            const dy = y - pointer.y;
            const d2 = dx * dx + dy * dy;
            if (d2 < 32000) glow += (1 - d2 / 32000) * 0.75;
          }
          const a = 0.07 + glow * 0.5;
          ctx.fillStyle = glow > 0.04 ? `rgba(243,181,98,${Math.min(a, 0.7).toFixed(3)})` : "rgba(255,255,255,0.07)";
          ctx.fillRect(x, y, 1.6, 1.6);
        }
      }
    };

    const loop = (now) => {
      draw(now);
      raf = requestAnimationFrame(loop);
    };

    const play = () => {
      if (running || reduceMotion.matches || !visible || document.hidden) return;
      running = true;
      raf = requestAnimationFrame(loop);
    };

    const pause = () => {
      running = false;
      cancelAnimationFrame(raf);
    };

    resize();
    window.addEventListener("resize", () => {
      clearTimeout(resize.t);
      resize.t = setTimeout(resize, 120);
    }, { passive: true });

    if (finePointer.matches) {
      hero.addEventListener("pointermove", (e) => {
        const rect = hero.getBoundingClientRect();
        pointer.tx = e.clientX - rect.left;
        pointer.ty = e.clientY - rect.top;
        if (pointer.x < -9000) { pointer.x = pointer.tx; pointer.y = pointer.ty; }
      });
      hero.addEventListener("pointerleave", () => { pointer.tx = pointer.ty = pointer.x = pointer.y = -9999; });
    }

    if ("IntersectionObserver" in window) {
      new IntersectionObserver(([entry]) => {
        visible = entry.isIntersecting;
        visible ? play() : pause();
      }).observe(hero);
    }
    document.addEventListener("visibilitychange", () => (document.hidden ? pause() : play()));
    reduceMotion.addEventListener("change", () => (reduceMotion.matches ? (pause(), draw(performance.now())) : play()));
    play();
  }
})();
